using FavoriteBook.DAL;
using FavoriteBook.Models;
using FavoriteBook.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
            //var myBooks = _db.Books.Where(u => u.Users.Any(i => i.Id == myUserId)).ToList();
            var myBooks = _service.GetBookByOwner(myUserId);
            return View(myBooks);
        }
        public IActionResult UserListBooks(string searchString, string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            
            var myUserId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(s => s.Value).FirstOrDefault();
            //var myBooks = _db.Books.Where(u => u.Users.Any(i => i.Id == myUserId)).ToList();
            
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

            return View(myBooks);
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

        //public IActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult Create(Book model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var myUserId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(s => s.Value).FirstOrDefault();
        //        var newBook = _service.AddBook(model, myUserId);
        //        return RedirectToAction("Index", "Private");
        //    }

        //    return View(model);
        //}


        //public IActionResult AddUserBook(int id)
        //{
        //    if (id == 0)
        //    {
        //        return NotFound();
        //    }

        //    var book = _service.GetBookById(id);
        //    if (book == null)
        //    {
        //        return RedirectToAction("Error", "Auth");
        //    }
        //    return View(book);
        //}

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

        //[Authorize(Roles = "Admin")]
        //public IActionResult Delete(int id)
        //{
        //    if (id == 0)
        //    {
        //        return NotFound();
        //    }
        //    var myUserId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(s => s.Value).FirstOrDefault();
        //    var book = _service.GetById(id, myUserId);
        //    if (book == null)
        //    {
        //        return RedirectToAction("Error", "Auth");
        //    }
        //    return View(book);
        //}

        //[HttpPost]
        //public IActionResult DeleteBook(Book book)
        //{
        //    var myUserId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(s => s.Value).FirstOrDefault();
        //    _service.DeleteBook(book.BookId, myUserId);
        //    return RedirectToAction("Index", "Private");
        //}

        //[Authorize(Roles = "Admin")]
        //public IActionResult Update(int id)
        //{
        //    if (id == 0)
        //    {
        //        return NotFound();
        //    }
        //    var myUserId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(s => s.Value).FirstOrDefault();
        //    var book = _service.GetById(id, myUserId);
        //    if (book == null)
        //    {
        //        return RedirectToAction("Error", "Auth");
        //    }
        //    return View(book);
        //}

        //[HttpPost]
        //public IActionResult Update(int id, Book model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var newBook = _service.UpdateBook(id, model);
        //        return RedirectToAction("Index", "Private");
        //    }

        //    return View(model);
        //}

        [Authorize(Roles = "Admin")]
        public IActionResult IsRead(int id)
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
        public IActionResult IsRead(int id, Book value)
        {
            var myUserId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(s => s.Value).FirstOrDefault();
            _service.IsRead(id, value.IsRead, myUserId);
            return RedirectToAction("Index", "Private");
        }


    }

}
