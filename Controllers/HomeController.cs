using Application.Feature.Lawyer.Query;
using Application.Feature.Support.Command;
using Lawer.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Lawer.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMediator _mediator;



        public HomeController(ILogger<HomeController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
       
        }

        public async Task<IActionResult> Index()
        {
            
            return View();
        }

        public async Task<IActionResult> LawyersList()
        {
          
            return View();
        }
        public async Task<IActionResult> GetList(int page = 1, string? search = null, int pageSize = 10)
        {
            var result = await _mediator.Send(new GetLawyersQuery()
            {
                PageNumber = page,
                PageSize = pageSize,
                Search = search,
                DisablePaging=false
            });
            ViewBag.search = search;
            ViewBag.pageSize = pageSize;
            return PartialView(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult ContactUs()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ContactUs([FromBody]SendFeedBackCommand command)
        {
            var result=await _mediator.Send(command);
            return Ok(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<IActionResult> Test(GetLawyersQuery query)
        {
            var result = await _mediator.Send(query);
            return View(result);
        }
        public async Task<IActionResult> GetLawyersList(int page = 1, string? search = null, int pageSize = 15)
        {
            var result = await _mediator.Send(new GetLawyersQuery()
            {
                PageNumber = page,
                PageSize = pageSize,
                Search = search,
                DisablePaging = false
            });
            ViewBag.search = search;
            ViewBag.pageSize = pageSize;
            return PartialView(result);
        }
    }
}
