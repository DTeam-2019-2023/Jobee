using Microsoft.AspNetCore.Mvc;

namespace Jobee.Views.Shared
{
    [Route("/Error")]
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("{statusCode}")]
        public IActionResult Index(int statusCode)
        {
            return View();
        }
    }
}
