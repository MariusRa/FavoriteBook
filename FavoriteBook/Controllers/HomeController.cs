using FavoriteBook.Models;
using FavoriteBook.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FavoriteBook.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBookService _service;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IBookService service)
        {
            _logger = logger;
            _service = service;
        }

        public IActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var myBooks = _service.GetAllBooks(1, 20);

            if (!String.IsNullOrEmpty(searchString))
            {
                myBooks = myBooks.Where(b => b.Author.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    myBooks = myBooks.OrderByDescending(b => b.Title);
                    break;

                default:
                    myBooks = myBooks.OrderBy(b => b.Title);
                    break;

            }

            return View(myBooks);
        }

     

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
