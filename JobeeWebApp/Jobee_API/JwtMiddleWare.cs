using Jobee_API.Controllers;
using Jobee_API.Models;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Jobee_API
{
    //public delegate void JwtAction();
    public class JwtMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly JwtTokenManager _tokenManager;
        ILogger<UsersController> _logger;
        private IDistributedCache _cache;
        public static JwtTokenManager _jwt;
        public JwtMiddleWare(RequestDelegate next, ILogger<UsersController> logger, IDistributedCache cache)
        {
            _next = next;
            _tokenManager = JwtTokenManager.Instance;
            _logger = logger;
            _cache = cache;
        }
        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                var status = _cache.Get(token);

                if (status != null)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return;
                }
                //var identity = (ClaimsIdentity)context.User.Identity;
                //var claim = JwtTokenManager.Instance.GetPrincipalFromToken(token).FindFirst(ClaimTypes.Role);
                //identity.AddClaim(claim);
            }
            //_logger.LogInformation("Authorization {0}", token);
            //if (token != null)
            //{
            //    attachUserToContext(context, token);
            //}
            await _next(context);
        }
        //private void attachUserToContext(HttpContext context, string token)
        //{
        //    try
        //    {
        //        var jwtToken = _tokenManager.ValidateJwtToken(token);
        //        context.Items["UserPrincipal"] = _tokenManager.GetPrincipalFromToken(token);
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
    }
}
