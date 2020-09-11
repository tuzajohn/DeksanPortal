using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeksanPortal.Core.IServices;
using DeksanPortal.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DeksanPortal.Web.Controllers
{
    [Authorize(policy: "DeksanOnly")]
    public class BooksController : Controller
    {
        private readonly IGlobalService<Library> _libraryService;
        public BooksController(IGlobalService<Library> libraryService)
        {
            _libraryService = libraryService;
        }

        public async Task<IActionResult> Index(string id)
        {
            var book = await _libraryService.Get(id);

            if (book == default)
            {
                ViewData["Error"] = "File not found";
            }

            return View(book);
        }
    }
}
