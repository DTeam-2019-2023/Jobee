using Microsoft.AspNetCore.Mvc;

namespace Jobee.Views.Shared
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
