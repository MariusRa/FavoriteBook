using FavoriteBook.Models;
using FavoriteBook.Services;
using FavoriteBook.viewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;

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

        // booklist'as su galimybe filtruoti pagal titel, genre, author
        public IActionResult Index(string searchString, string bookTitle, string bookGenre)
        {
            var myBooks = _service.GetAllBooks(1, 20);

            if (!String.IsNullOrEmpty(searchString))
            {
                myBooks = myBooks.Where(b => b.Author.ToLower().Contains(searchString));
            }

            if (!String.IsNullOrEmpty(bookTitle))
            {
                myBooks = myBooks.Where(b => b.Title.ToLower().Contains(bookTitle));
            }

            if (!String.IsNullOrEmpty(bookGenre))
            {
                myBooks = myBooks.Where(b => b.Genre.ToLower().Contains(bookGenre));
            }

            var user = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(s => s.Value).FirstOrDefault();
            

            var model = new List<UserBooksViewModel>();
            
            foreach (var book in myBooks)
            {
                var userBooksViewModel = new UserBooksViewModel
                {
                    BookId = book.BookId,
                    BookAuthor = book.Author,
                    BookTitle = book.Title,
                    BookGenre = book.Genre
                };

                if (_service.GetById(book.BookId, user) != null)
                {
                    userBooksViewModel.IsSelected = true;
                }
                else
                {
                    userBooksViewModel.IsSelected = false;
                }
                
                model.Add(userBooksViewModel);
            }

            return View(model);
        }

        // detali info apie knygą
        public IActionResult BookDetails(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
          
            var book = _service.GetBookById(id);
            if (book == null)
            {
                return RedirectToAction("Error", "Auth");
            }
            
            return View(book);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
