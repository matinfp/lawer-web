using Application.Common.Execption;
using Application.Feature.ForgetPassword.Command;
using Application.Models;
using Domain.Models.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
namespace Lawer.Controllers
{
    public class ForgetPasswordController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMediator _mediator;

        public ForgetPasswordController(UserManager<User> userManager, SignInManager<User> signInManager, IMediator mediator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mediator = mediator;
        }



        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPassWordCommand command)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(command);
                //HttpContext.Session.SetString("OtpUserId", result.Id);
                ViewBag.PhoneNumber = command.PhoneNumber;
                return PartialView("VerifyOtp");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> VerifyOtp(string phoneNumber, string otp)
        {
            var user = _userManager.Users
                .FirstOrDefault(u => u.PhoneNumber == phoneNumber);
            if (user == null || user.CodeExpireAt < DateTime.UtcNow)
            {
                return Json(new { success = false });
            }
            if (user.Code == int.Parse(otp))
            {

                return Json(new { success = true });
            }
            return Json(new { success = false });
        }


        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }


        // -----------------------------
        // STEP 3: Reset Password Page
        // -----------------------------
        [HttpGet]
        public IActionResult ResetPassword(string phoneNumber)
        {
            return View(model: phoneNumber);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(
            string phoneNumber,
            string newPassword)
        {
            var user = _userManager.Users
                .FirstOrDefault(u => u.PhoneNumber == phoneNumber);

            if (user == null || !user.PhoneNumberConfirmed==false)
                return BadRequest();

            await _userManager.RemovePasswordAsync(user);
            await _userManager.AddPasswordAsync(user, newPassword);


            await _userManager.UpdateAsync(user);

            return RedirectToAction("Login");
        }

        //[HttpGet]
        //public IActionResult ResetPassword(string code = null)
        //{
        //    if (code == null)
        //    {
        //        throw new ApplicationException("A code must be supplied for password reset.");
        //    }
        //    var model = new ResetPassWordViewModel { Code = code };
        //    return View(model);
        //}


        //[HttpPost]
        //public async Task<IActionResult> ResetPassword(ResetPassWordViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    var user = await _userManager.FindByEmailAsync(model.Email);
        //    if (user == null)
        //    {
        //        return RedirectToAction(nameof(ResetPasswordConfirmation));
        //    }

        //    var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
        //    if (result.Succeeded)
        //    {
        //        return RedirectToAction(nameof(ResetPasswordConfirmation));
        //    }

        //    foreach (var error in result.Errors)
        //    {
        //        ModelState.AddModelError(string.Empty, error.Description);
        //    }
        //    return View();
        //}



        //[HttpGet]
        //public IActionResult ResetPasswordConfirmation()
        //{
        //    return View();
        //}
    }
}
