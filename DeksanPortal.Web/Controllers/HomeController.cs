using DeksanPortal.Core.IServices;
using DeksanPortal.Data.Enums;
using DeksanPortal.Data.Helpers;
using DeksanPortal.Data.Helpers.Extensions;
using DeksanPortal.Data.Models;
using DeksanPortal.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeksanPortal.Web.Controllers
{
    [Authorize(policy: "DeksanOnly")]
    public class HomeController : Controller
    {
        private readonly IGlobalService<Account> _accountService;
        private readonly IGlobalService<User> _userService;
        private readonly IGlobalService<Library> _libraryService;
        private readonly IGlobalService<Class> _classService;
        private readonly IGlobalService<Follow> _followService;
        private readonly IGlobalService<Category> _categoryService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession Session => _httpContextAccessor.HttpContext.Session;

        public HomeController(IGlobalService<Account> accountService, IGlobalService<User> userService, IGlobalService<Library> libraryService,
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




        public IActionResult Interim()
        {
            return View();
        }


        public IActionResult LoadLibrary(int page)
        {
            var books = _libraryService.SearchQ(x => x.EducationLevel == User.GetEducation())
                .OrderByDescending(x => x.CreatedOn)
                .Skip(page * 10)
                .Take(10)
                .ToList();

            if (User.GetEducation() != Education.STAY_AT_HOME)
            {
                books = books.Where(x => x.ClassId == User.GetClass())
                    .ToList();
            }

            var bookList = new List<dynamic>();

            foreach (var book in books)
            {
                var _book = new
                {
                    book.Name,
                    book.Id,
                    book.Description,
                    book.ThumbnailUrl,
                    book.ResourceUrl
                };

                bookList.Add(_book);
            }

            return Ok(bookList);
        }


        public async Task<IActionResult> SaveAbout(AboutViewModel model)
        {
            if (model.EducationLevel == default)
            {
                return Ok(new
                {
                    code = -1,
                    message = "Select highest level of education"
                });
            }

            if (model.EducationLevel != Education.STAY_AT_HOME)
            {
                if (model.EducationClass == default)
                {
                    return Ok(new
                    {
                        code = -1,
                        message = "Select the class you are attending"
                    });
                }
            }

            var user = await _userService.Get(User.GetUserId());

            user.Address = model.Address;
            user.Occupation = model.Occupation;
            user.DateOfBirth = model.Dob;
            user.Education = model.EducationLevel;
            user.ClassId = model.EducationClass;

            Session.SetString("user", user.ToJson());

            return Ok(new
            {
                code = 0
            });
        }
        public async Task<IActionResult> SaveAll(string categories)
        {
            var cats = categories.Split(',')
                .ToList();

            var user = Session.GetString("user")
                .FromJson<User>();


            var userData = await _userService.Get(user.Id);

            userData.Address = user.Address;
            userData.Occupation = user.Occupation;
            userData.DateOfBirth = user.DateOfBirth;
            userData.Education = user.Education;
            userData.ClassId = user.ClassId;

            await _userService.Update(userData);

            foreach (var category in cats)
            {
                var following = new Follow
                {
                    Id = Support.Id,
                    UserId = userData.Id,
                    CreatedOn = DateTime.UtcNow,
                    LibraryId = category
                };

                await _followService.Add(following);
            }


            return Ok(new
            {
                code = 0,
                message = "update"
            });
        }
        public IActionResult GetClass(Education lvl)
        {
            var classes = _classService.SearchQ(x => x.EducationLevel == lvl)
                .OrderBy(x => x.CreatedOn)
                .ToList();
            return Ok(classes);
        }
        public IActionResult GetCategories()
        {
            var categories = _categoryService.SearchQ(x => true)
                .OrderByDescending(x => x.CreatedOn)
                .ToList();

            return Ok(categories);
        }
    }
}
