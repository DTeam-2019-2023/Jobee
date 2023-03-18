using Jobee_API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Jobee_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TbAccountController : ControllerBase
    {
        private readonly Project_JobeeContext _dbContext;
        private IConfiguration _config;
        private readonly ILogger<UsersController> _logger;
        private JwtTokenManager tokenManager;
        private IDistributedCache _cache;
        public TbAccountController(IConfiguration config, Project_JobeeContext dbContext, ILogger<UsersController> logger, IDistributedCache cache)
        {
            _config = config;
            _dbContext = dbContext;
            _logger = logger;
            tokenManager = JwtTokenManager.Instance;
            _cache = cache;
        }

        [HttpGet]
        [Route("GetAll")]
        public ActionResult<List<TbAccount>> GetAccounts()
        {
            if(_dbContext.TbAccounts.Any())
            {
                return _dbContext.TbAccounts.ToList();
            }
            return null;
        }

        
      
    }
}
