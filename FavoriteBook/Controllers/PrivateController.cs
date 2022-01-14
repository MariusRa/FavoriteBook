using FavoriteBook.DAL;
using FavoriteBook.Models;
using FavoriteBook.Services;
using FavoriteBook.viewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;


namespace FavoriteBook.Controllers
{
    [Authorize]
    public class PrivateController : Controller
    {
        private readonly IBookService _service;
        private readonly ApplicationDbContext _db;

        public PrivateController(IBookService service, ApplicationDbContext db)
        {
            _service = service;
            _db = db;
        }
        public IActionResult Index()
        {
            var myUserId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(s => s.Value).FirstOrDefault();
            
            var myBooks = _service.GetBookByOwner(myUserId);
            return View(myBooks);
        }
        public IActionResult UserListBooks(string searchString, string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            
            var myUserId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(s => s.Value).FirstOrDefault();
            
            
            var myBooks = _service.GetBookByOwner(myUserId);

            if (!String.IsNullOrEmpty(searchString))
            {
                myBooks = myBooks.Where(b => b.Title.ToLower().Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    myBooks = myBooks.OrderByDescending(s => s.Title);
                    break;

                default:
                    myBooks = myBooks.OrderBy(s => s.Title);
                    break;
            }

            var model = new List<BookMembershipViewModel>();

            foreach (var book in myBooks)
            {
                var bookMembershipViewModel = new BookMembershipViewModel
                {
                    BookId = book.BookId,
                    Title = book.Title,
                    Genre = book.Genre,
                    Author = book.Author,
                    IsBookRead = _db.Memberships.FirstOrDefault(x => x.Book.BookId == book.BookId && x.User.Id == myUserId).IsBookRead
                };

                model.Add(bookMembershipViewModel);
            }
            

            return View(model);
        }

        public IActionResult BookDetails(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var myUserId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(s => s.Value).FirstOrDefault();
            var book = _service.GetById(id, myUserId);
            if (book == null)
            {
                return RedirectToAction("Error", "Auth");
            }
            return View(book);
        }

 
        [HttpPost]
        public IActionResult AddUserBook(int id)
        {
            var user = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(s => s.Value).FirstOrDefault();

            var book = _service.AddBookUser(id, user);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult DeleteUserBook(int id)
        {
            var user = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(s => s.Value).FirstOrDefault();
            var book = _service.DeleteBookUser(id, user);

            return RedirectToAction("UserListBooks", "Private");
        }

        [HttpPost]
        public IActionResult IsBookRead(int id, Membership value)
        {
            var myUserId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(s => s.Value).FirstOrDefault();

            var member = _db.Memberships.FirstOrDefault(x => x.Book.BookId == id && x.User.Id == myUserId);
                      
            if (member.IsBookRead == false)
            {
                _service.IsBookRead(id, value.IsBookRead = true, myUserId);
            }
            else
            {
                _service.IsBookRead(id, value.IsBookRead = false, myUserId);
            }

            return RedirectToAction("UserListBooks", "Private");
        }
    }
}
