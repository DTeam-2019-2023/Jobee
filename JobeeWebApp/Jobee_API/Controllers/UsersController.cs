using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Jobee_API.Tools;
using Jobee_API.Models;
using Jobee_API.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Principal;
using Microsoft.IdentityModel.JsonWebTokens;
using NuGet.Common;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;

namespace Jobee_API.Controllers
{
    public interface IUserController
    {
        public Task<IActionResult> LoginAction([FromBody] User _loginReq);
        public Task<IActionResult> LogOutAction();
        //public Task<IActionResult> SignUpAction(TbAccount account);
        //public Task<IActionResult> ChangePasswordAction(string oldPassword, string newPassword);
        //public Task<IActionResult> ChangeMailAction();
        //public Task<IActionResult> VerifyConfirmAction();
        //public Task<IActionResult> ForgotPasswordAction();
    }

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase, IUserController
    {
        private readonly Project_JobeeContext _dbContext;
        private IConfiguration _config;
        private readonly ILogger<UsersController> _logger;
        private JwtTokenManager tokenManager;
        private IDistributedCache _cache;
        public UsersController(IConfiguration config, Project_JobeeContext dbContext, ILogger<UsersController> logger, IDistributedCache cache)
        {
            _config = config;
            _dbContext = dbContext;
            _logger = logger;
            tokenManager = JwtTokenManager.Instance;
            _cache = cache;
        }

        [HttpPost]
        [Route("signup")]
        [AllowAnonymous]
        public async Task<IActionResult> signupAccount([FromBody] User user)
        {
            string userid = Guid.NewGuid().ToString();
            var dbUser = _dbContext.TbAccounts.Where(u => u.Username.Equals(user.username)).SingleOrDefault();
            if (dbUser != null)
            {
                return BadRequest("Username already exist");
            }
            user.password = HashPassword.hashPassword(user.password);
            TbAccount account = new TbAccount()
            {
                Id = userid,
                IdtypeAccount = "emp",
                Username = user.username,
                Passwork = user.password,
            };

            _dbContext.TbAccounts.Add(account);
            _dbContext.SaveChanges();
            return Ok(user.username);
        }

        private TbAccount Authentication(User user)
        {
            //deconstruct data user
            var userdata = (username: user.username,
                            passwordHash: HashPassword.hashPassword(user.password));
            //get contruct
            var (username, password) = userdata;

            //Check user is valid
            var userStamp = _dbContext.TbAccounts.FirstOrDefault(
                u => u.Username.Equals(username) &&
                     u.Passwork.Equals(password));

            if (userStamp != null)
            {
                return userStamp;
            }
            return null!;
        }

        

        [HttpPost("logout")]
        [Authorize(Roles = "emp,ad")]
        public async Task<IActionResult> LogOutAction()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await _cache.SetStringAsync(token, "logout");
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public Task<IActionResult> LoginAction([FromBody] User _loginReq)
        {
            IActionResult res = Unauthorized("Login Fail");
            var user = Authentication(_loginReq);
            try
            {
                if (user != null)
                {
                    var token = tokenManager.GenerateJwtToken(
                        issuer: _config["Jwt:Issuer"],
                        audience: _config["Jwt:Audience"],
                        expires: DateTime.Now.AddDays(60d),
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Role, user.IdtypeAccount),
                        new Claim("id", user.Id)
                        );
                    _logger.LogInformation("Login successfuly \n {0}", user.ToString());
                    res = Ok(new { token = token, type = user.IdtypeAccount });
                }
                else
                {
                    _logger.LogInformation("Login Fail \n username: {0} \n password: {1}", _loginReq.username, _loginReq.password);
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }

            return Task.FromResult(res);
        }

        [HttpPut]
        [Route("/api/User/Update")]
        [Authorize(Roles = "emp,ad")]
        public async Task<ActionResult<User>> ChangePasswordAction([FromBody] User model)
        {
            string iduser = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

            var u = _dbContext.TbAccounts.Single(a => a.Id.Equals(iduser));
                u.Passwork = model.password;
            u.Passwork =  Tools.HashPassword.hashPassword(u.Passwork);
            _dbContext.SaveChanges();
            return model;
        }

        [HttpPut]
        [Route("/api/User/ChangeEmail")]
        //[Authorize(Roles = "emp,ad")]
        public async Task<ActionResult<Profile>> ChangeEmailAction([FromBody] Profile model)
        {
            string iduser = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

            var u = _dbContext.TbProfiles.Single(a => a.Id.Equals(iduser));
            u.Email = model.Email;
            _dbContext.SaveChanges();
            return model;
        }
    }
}
