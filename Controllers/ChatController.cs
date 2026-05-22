using Microsoft.AspNetCore.Mvc;

namespace Lawer.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
