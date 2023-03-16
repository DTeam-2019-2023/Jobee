using Microsoft.AspNetCore.Mvc;

namespace Jobee.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
