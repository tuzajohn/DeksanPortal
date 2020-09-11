using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeksanPortal.Core.IServices;
using DeksanPortal.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeksanPortal.Web.Controllers
{
    [Authorize(policy: "DeksanOnly")]
    public class ProfileController : Controller
    {
        private readonly IGlobalService<Account> _accountService;
        private readonly IGlobalService<User> _userService;
        private readonly IGlobalService<Library> _libraryService;
        private readonly IGlobalService<Class> _classService;
        private readonly IGlobalService<Follow> _followService;
        private readonly IGlobalService<Category> _categoryService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession Session => _httpContextAccessor.HttpContext.Session;
        public ProfileController(IGlobalService<Account> accountService, IGlobalService<User> userService, IGlobalService<Library> libraryService,
            IGlobalService<Class> classService, IGlobalService<Category> categoryService, IHttpContextAccessor httpContextAccessor, IGlobalService<Follow> followService)
        {
            _accountService = accountService;
            _userService = userService;
            _libraryService = libraryService;
            _classService = classService;
            _categoryService = categoryService;
            _httpContextAccessor = httpContextAccessor;
            _followService = followService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
