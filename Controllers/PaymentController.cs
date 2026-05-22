using Application.Feature.Payments.Command;
using Application.Models.ParBadDto;
using Domain.Models.Case;
using Domain.Models.Lawyer;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parbad;

namespace Lawer.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ILawyerProfileRepository _LawyerRepository;
        private readonly ICasesRepository _CasesRepository;
        private readonly IMediator _mediator;
        public PaymentController(ICasesRepository casesRepository, ILawyerProfileRepository lawyerRepository, IMediator mediator)
        {
            _CasesRepository = casesRepository;
            _LawyerRepository = lawyerRepository;
            _mediator = mediator;
        }
        [HttpGet]
        public IActionResult Pay()
        {
            return View(new PayViewModel());
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PayRequest([FromBody] PaymentCommand command)
        {
            var callbackUrl = Url.Action("Verify", "PaymentTest", null, Request.Scheme);
            command.CallbackUrl = callbackUrl;
            if (ModelState.IsValid)
            {
                return await _mediator.Send(command);
            }
            return Ok();
        }

        [HttpGet, HttpPost]
        public async Task<IActionResult> Verify(string CaseId)
        {

            var result = await _mediator.Send(new VerifyCommand() { CaseId = CaseId });
            // Check if the invoice is new or it's already processed before.
            if (result.IsSuccess == false)
            {
                // You can also see if the invoice is already verified before.

                return Content("The payment is already processed before.");
            }

            // This is an example of cancelling an invoice when you think that the payment process must be stopped.
            if (result.TrakingnNumberIsExist == false)
            {
                return View("CancelResult", "پروسه خرید شما با خطا مواجه شد لطفا دوباره سعی کنید ");
            }


            // Note: Save the verifyResult.TransactionCode in your database.

            return View(result.PaymentVerifyResult);
        }

        [HttpGet]
        public IActionResult Refund()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> Refund(RefundViewModel viewModel)
        //{
        //    //var result = await _onlinePayment.RefundCompletelyAsync(viewModel.TrackingNumber);

        //    //return View("RefundResult", result);
        //}
    }
}
