using Application.Common.Execption;
using Application.Feature.Authentication.Command;
using Application.Models;
using Application.Services.SmsServices;
using Azure.Core;
using Domain.Models.Lawyer;
using Domain.Models.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace Lawer.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ISMSService _smsSender;
        private readonly IMediator _mediator;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ISMSService smsSender, IMediator mediator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _smsSender = smsSender;
            _mediator = mediator;
        }
        #region Register
        public IActionResult Register()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterCommand command)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _mediator.Send(command);
                    
                    return RedirectToAction(nameof(VerifyPhoneNumber), new { UserId = result.UserId });

                }
                catch (CustomExecption ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    var vm = new RegisterViewModel
                    {
                        Name = command.Name,
                        LastName = command.LastName,
                        Email = command.Email,
                        PhoneNumber = command.PhoneNumber,
                        Password = command.Password
                    };
                    return View(vm);
                }
            }
            return View();
        }
        #endregion





        //login client logic
        #region Login

        public IActionResult Login(string returnUrl = null)
        {
            //return url for continue shopping for without login users || ejorayii negah midare akharin jayi ke bodi ro fekr konam
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {

            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.PhoneNumber == model.PhoneNumber);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "کاربری با این شماره تلفن پیدا نشد");
                return View(model);
            }
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(user.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToLocal(returnUrl);
                }
                if (result.IsLockedOut)
                {
                    return View("Lockout");
                }
                else
                {

                    ModelState.AddModelError(string.Empty, "رمز عبور صحیح نمیباشد");
                    return View(model);
                }
            }

            return View();
        }
        #endregion




        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        public async Task<IActionResult> VerifyPhoneNumber(string userId, bool? lawyer)
        {
            var model = new VerifyPhoneNumberViewModel { UserId = userId, Lawyer = lawyer };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> VerifyPhoneNumber([FromBody] VerifyPhoneNumberViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user.CodeExpireAt < DateTime.UtcNow)//برسی تایم 
                {
                    return BadRequest("کد منقضی شده است، لطفاً دوباره درخواست دهید.");
                }
                if (user == null)
                {
                    return NotFound("user not found");
                }
                var result = await _userManager.ChangePhoneNumberAsync(user, user.PhoneNumber, model.Code);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return Ok(new { success = true, redirectUrl = Url.Action("Index", "Home") });
                }
                ModelState.AddModelError(string.Empty, "مشکل در تایید کد !!!!");
            }
            return View(model);
        }
        public IActionResult Terms()
        {
            return View();
        }

    }
}