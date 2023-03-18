using Jobee_API.Entities;
using Jobee_API.Models;
using Jobee_API.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Jobee_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ad")]
    public class AdminController : ControllerBase
    {
        private readonly Project_JobeeContext _dbContext;

        public AdminController(Project_JobeeContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TbAccount>>> GetTbAccountforManagerUser()
        {
            return await _dbContext.TbAccounts.ToListAsync();
        }

        [HttpGet]
        [Route("getAllCertificate")]
        [Authorize(Roles = "ad")]
        public async Task<ActionResult<IEnumerable<Certificate>>> GetCertificates()
        {
            return await _dbContext.Certificates.ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> signupAccountAdmin([FromBody] User user)
        {
            string userid = Guid.NewGuid().ToString();
            var dbUser = _dbContext.TbAccounts.Where(u => u.Username.Equals(user.username) && u.IdtypeAccount.Equals("ad")).SingleOrDefault();
            if (dbUser != null)
            {
                return BadRequest("Username already exist");
            }
            user.password = HashPassword.hashPassword(user.password);
            TbAccount account = new TbAccount()
            {
                Id = userid,
                IdtypeAccount = "ad",
                Username = user.username,
                Passwork = user.password
            };

            _dbContext.TbAccounts.Add(account);
            _dbContext.SaveChanges();
            return Ok(account);
        }

        [HttpPut]
        [Route("VerifyCertificate/{id}")]
        [Authorize(Roles = "ad")]
        public async Task<IActionResult> VerifyCertificate(string id)
        {
            var existIdCer = await _dbContext.Certificates.FindAsync(id);
            if (existIdCer == null)
            {
                return BadRequest();
            }

            existIdCer.IsVertify = true;
            _dbContext.Update(existIdCer);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CertificateExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(existIdCer);
        }

        private bool CertificateExists(string id)
        {
            return _dbContext.Certificates.Any(e => e.Id == id);
        }

        [HttpGet]
        [Route("GetVerifies")]
        [AllowAnonymous]
        public ActionResult<List<object>> GetVerifies()
        {
            var result = (from certs in _dbContext.Certificates
                          join cvs in _dbContext.TbCvs on certs.Idcv equals cvs.Id
                          join acc in _dbContext.TbProfiles on cvs.Idaccount equals acc.Idaccount
                          select new
                          {
                              FullName = acc.FirstName + " " + acc.LastName,
                              Name = certs.Name,
                              StartDate = certs.StartDate,
                              EndDate = certs.EndDate,
                              Description = certs.Description,
                              Url = certs.Url
                          }).ToList<object>();
            return result;
        }
    }
}
