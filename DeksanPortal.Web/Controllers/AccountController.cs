using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DeksanPortal.Core.IServices;
using DeksanPortal.Data.Helpers;
using DeksanPortal.Data.Helpers.Extensions;
using DeksanPortal.Data.Models;
using DeksanPortal.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeksanPortal.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IGlobalService<Account> _accountService;
        private readonly IGlobalService<User> _userService;
        private readonly IGlobalService<Follow> _followService;

        public AccountController(IGlobalService<User> userService, IGlobalService<Account> accountService, IGlobalService<Follow> followService)
        {
            _accountService = accountService;
            _userService = userService;
            _followService = followService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Message = "Fill in all the fields";
                return View(model);
            }

            var password = model.Password.ToSHA256();

            var account = _accountService.SearchQ(x => x.Email == model.Email && x.Password == password)
                .FirstOrDefault();

            if (account == null)
            {
                model.Message = "Wrong email or password";
                return View(model);
            }


            var user = _userService.SearchQ(x => x.AccountId == account.Id)
                .FirstOrDefault();

            if (user == null)
            {
                model.Message = "Wrong email or password";
                return View(model);
            }

            var follow = _followService.SearchQ(x => x.UserId == user.Id).ToList();


            await RegisterClaimsAsync(user);

            if (account.Type == Data.Enums.AccountStatus.ADMIN)
            {
                return RedirectToAction("index", "dashboard");
            }
            if (follow.Count == 0)
            {
                return RedirectToAction("interim", "home");
            }

            return RedirectToAction("index", "home");
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Message = "Fill in all the fields";
                return View(model);
            }

            var accounts = _accountService.SearchQ(x => x.Email == model.Email)
                .ToList();

            if (accounts.Count > 0)
            {
                model.Message = "Email not avaible";
                return View(model);
            }

            if (model.Password != model.RepPassword)
            {
                model.Message = "Paswords not matching";
                return View(model);
            }

            model.Password = model.Password.ToSHA256();

            var account = new Account
            {
                Id = Support.Id,
                Email = model.Email,
                Password = model.Password
            };

            account = await _accountService.Add(account);
            if (account == null)
            {
                model.Message = "Registration failed";
                return View(model);
            }

            var user = new User
            {
                Id = Support.Id,
                AccountId = account.Id,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            user = await _userService.Add(user);

            if (user == null)
            {
                await _accountService.Delete(account);
                model.Message = "Registration failed";
                return View(model);
            }
            await RegisterClaimsAsync(user);

            return RedirectToAction("interim", "home");
        }

        public IActionResult Create()
        {
            return Ok();
        }

        private async Task RegisterClaimsAsync(User user)
        {
            try
            {
                var claims = new List<Claim>
                {
                    new Claim("UserId", user.Id),
                    new Claim("Name", user.Name),
                    new Claim("Education", user.Education.ToString()),
                    new Claim("Class", user?.ClassId??"")
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            }
            catch { }
        }
    }
}
