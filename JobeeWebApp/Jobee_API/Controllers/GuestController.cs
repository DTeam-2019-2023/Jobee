using Jobee_API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Jobee_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestController : ControllerBase
    {
        private readonly Project_JobeeContext _dbContext;
        private IConfiguration _config;
        private readonly ILogger<UsersController> _logger;
        private JwtTokenManager tokenManager;
        private IDistributedCache _cache;
        public GuestController(IConfiguration config, Project_JobeeContext dbContext, ILogger<UsersController> logger, IDistributedCache cache)
        {
            _config = config;
            _dbContext = dbContext;
            _logger = logger;
            tokenManager = JwtTokenManager.Instance;
            _cache = cache;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("UserProfile")]
        public ActionResult<(TbProfile, TbCv, List<Education>, List<Project>, List<Certificate>, List<Activity>, List<Award>)> GetUser(string username)
        {
            var account = _dbContext.TbAccounts.FirstOrDefault(i => i.Username == username);
            if (account != null)
            {
                var profile = _dbContext.TbProfiles.FirstOrDefault(x => x.Idaccount == account.Id);
                var cv = _dbContext.TbCvs.FirstOrDefault(i => i.Idaccount == account.Id);
                var edu = _dbContext.Educations.Where(i => i.Idcv == cv.Id).ToList();
                var project = _dbContext.Projects.Where(i => i.Idcv == cv.Id).ToList();
                var certificate = _dbContext.Certificates.Where(i => i.Idcv == cv.Id).ToList();
                var activity = _dbContext.Activities.Where(i => i.Idcv == cv.Id).ToList();
                var award = _dbContext.Awards.Where(i => i.Idcv == cv.Id).ToList();
                return (profile, cv, edu, project, certificate, activity, award);
            }
            return NotFound();
        }
    }
}
