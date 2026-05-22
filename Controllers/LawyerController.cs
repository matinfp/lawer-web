using Application.Feature.Lawyer.Query;

using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lawer.Controllers
{
    public class LawyerController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMediator _mediator;


        public LawyerController(ILogger<HomeController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        [HttpGet("/lawyer/{slug}")]
        public async Task<IActionResult> Lawyer(string slug)
        {
            var result = await _mediator.Send(new GetLawyerPageQuery(slug));

            if (result == null)
            {
                return NotFound(); // یا ریدایرکت کن به صفحه 404 سفارشی
            }

            return View(result);
        }
        public async Task<IActionResult> CreateCase()
        {
            
            return View();
        }
        public async Task<IActionResult> LawyerInfo()
        {
            
            return PartialView();
        }
        public async Task<IActionResult> Payment()
        {

            return View();
        }

    }
}
