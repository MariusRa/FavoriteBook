using FavoriteBook.Models;
using FavoriteBook.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;


namespace FavoriteBook.Controllers
{
    [Authorize]
    public class PrivateController : Controller
    {
        private readonly IBookService _service;

        public PrivateController(IBookService service)
        {
            _service = service;
        }
        public IActionResult Index()
        {
            var myUserId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(s => s.Value).FirstOrDefault();
            var myBooks = _service.GetBookByOwner(myUserId);
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Book model)
        {
            if (ModelState.IsValid)
            {
                var myUserId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(s => s.Value).FirstOrDefault();
                var newBook = _service.AddBook(model, myUserId);
                return RedirectToAction("Index", "Private");
            }
            
            return View(model);
        }

        public IActionResult Delete(int id)
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
        public IActionResult DeleteBook(Book book)
        {
            var myUserId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(s => s.Value).FirstOrDefault();
            _service.DeleteBook(book.BookId, myUserId);
            return RedirectToAction("Index", "Private");
        }

        public IActionResult Update(int id)
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
        public IActionResult Update(int id, Book model)
        {
            if (ModelState.IsValid)
            {
                var newBook = _service.UpdateBook(id, model);
                return RedirectToAction("Index", "Private");
            }

            return View(model);
        }

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
