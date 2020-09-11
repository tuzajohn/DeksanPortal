using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DeksanPortal.Core.IServices;
using DeksanPortal.Data.Enums;
using DeksanPortal.Data.Helpers;
using DeksanPortal.Data.Models;
using DeksanPortal.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace DeksanPortal.Web.Controllers
{
    [Authorize(policy: "DeksanOnly")]
    public class DashboardController : Controller
    {
        private readonly IGlobalService<Category> _categoryService;
        private readonly IGlobalService<Library> _libraryService;
        private readonly IGlobalService<CategoryLibrary> _categoryLibraryService;
        private readonly IGlobalService<Class> _classService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DashboardController(IGlobalService<Category> categoryService, IGlobalService<Library> libraryService, IGlobalService<Class> classService,
            IWebHostEnvironment webHostEnvironment, IGlobalService<CategoryLibrary> categoryLibraryService)
        {
            _categoryService = categoryService;
            _libraryService = libraryService;
            _classService = classService;
            _webHostEnvironment = webHostEnvironment;
            _categoryLibraryService = categoryLibraryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Library()
        {
            return View();
        }
        public IActionResult Users()
        {
            return View();
        }

        public async Task<IActionResult> AddCategory(string name, string description)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(description))
            {
                return Ok(new
                {
                    code = -1,
                    message = "Fill in all the fields"
                });
            }


            var category = new Category
            {
                Description = description,
                Id = Support.Id,
                Name = name
            };


            category = await _categoryService.Add(category);

            if (category == default)
            {
                return Ok(new
                {
                    code = -1,
                    message = "Failed to save"
                });
            }


            return Ok(new
            {
                code = 0,
                message = "Save successfully",
                data = category
            });
        }

        public async Task<IActionResult> AddClass(Education level, string name, string description)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(description) || level == default)
            {
                return Ok(new
                {
                    code = -1,
                    message = "Fill in all the fields"
                });
            }

            var classes = _classService.SearchQ(x => x.Name == name && x.EducationLevel == level)
                .ToList();

            if (classes.Count > 0)
            {
                return Ok(new
                {
                    code = -1,
                    message = "Name already in use"
                });
            }

            var @class = new Class
            {
                CreatedOn = DateTime.UtcNow,
                EducationLevel = level,
                Description = description,
                Id = Support.Id,
                Name = name
            };

            @class = await _classService.Add(@class);
            if (@class == default)
            {
                return Ok(new
                {
                    code = -1,
                    message = "Failed to add class"
                });
            }

            return Ok(new
            {
                code = 0,
                message = "Class added",
                data = @class
            });

        }

        public IActionResult GetClass(Education lvl)
        {
            var classes = _classService.SearchQ(x => x.EducationLevel == lvl)
                .OrderBy(x=>x.CreatedOn)
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

        [HttpPost]
        public async Task<IActionResult> AddBook(BookViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new
                {
                    code = -1,
                    message = "Fill in the required fields and provide image"
                });
            }

            var libraries = _libraryService.SearchQ(x => x.Name == model.Title)
                .ToList();

            if (libraries.Count > 0)
            {
                return Ok(new
                {
                    code = -1,
                    message = "Book exist"
                });
            }

            var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "assets/images/books");

            var filePath = Path.Combine(uploads, model.ThumbnailUrl.FileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await model.ThumbnailUrl.CopyToAsync(stream);

            var library = new Library
            {
                CreatedOn = DateTime.UtcNow,
                Description = model.Description,
                EducationLevel = model.educationLevel,
                LibraryType = LibraryType.PDF,
                Id = Support.Id,
                Name = model.Title,
                ClassId = model.ClassLevel
            };

            library.ThumbnailUrl = "/assets/images/books/" + model.ThumbnailUrl.FileName;

            filePath = Path.Combine(uploads, model.Resource.FileName);

            using var fileStream = new FileStream(filePath, FileMode.Create);
            await model.Resource.CopyToAsync(fileStream);

            library.ResourceUrl = "/assets/images/books/" + model.Resource.FileName; ;

            var categories = model.Categories.Split(new char[] { ',' });

            library.LibraryType = Path.GetExtension(model.Resource.FileName).Contains("pdf") ? LibraryType.PDF : LibraryType.EPUB;

            library = await _libraryService.Add(library);

            foreach (var cat in categories)
            {
                var libraryCateogies = new CategoryLibrary
                {
                    Id = Support.Id,
                    CategoryId = cat,
                    LibraryId = library.Id
                };

                libraryCateogies = await _categoryLibraryService.Add(libraryCateogies);
            }

            return Ok(new
            {
                code = 0,
                message = "Save book"
            });
        }

        public IActionResult GetBooks()
        {
            var booksFromLibrary = _libraryService
                .SearchQ(x => x.LibraryType == LibraryType.EPUB || x.LibraryType == LibraryType.PDF)
                .OrderByDescending(x=>x.CreatedOn)
                .ToList();

            var categories = _categoryService
                .SearchQ(x => true)
                .ToList();

            var bookList = new List<dynamic>();
            var index = 1;
            foreach (var bookFromLibrary in booksFromLibrary)
            {
                var libraryCategories = _categoryLibraryService
                    .SearchQ(x => x.LibraryId == bookFromLibrary.Id)
                    .ToList();

                var bookCategories = categories
                    .Where(x => libraryCategories.Any(y => y.CategoryId == x.Id))
                    .Select(x=>x.Name)
                    .ToList();

                var book = new
                {
                    bookFromLibrary.Id,
                    Position = index,
                    Title = bookFromLibrary.Name,
                    bookFromLibrary.Description,
                    Categories = string.Join(", ", bookCategories)
                };
                index++;


                bookList.Add(book);
            }

            return Ok(bookList);
        }
    }
}
