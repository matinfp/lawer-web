using Application.Features.Upload.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lawer.Controllers
{
    
    public class TestController : Controller
    {
        private readonly IMediator _mediator;

        public TestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public IActionResult Index()
        {
            return View();
        }

    }
}
