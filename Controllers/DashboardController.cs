using Microsoft.AspNetCore.Mvc;

namespace Lawer.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
