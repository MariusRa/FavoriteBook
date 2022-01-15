using FavoriteBook.Models;
using FavoriteBook.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FavoriteBook.Controllers
{
    [Authorize(Roles = "Admin, BookManageUser")]
    public class BookController : Controller
    {
        private readonly IBookService _service;

        public BookController(IBookService service)
        {
            _service = service;
        }
        
        public IActionResult ListBooks()
        {
            return View(_service.GetAllBooks(1, 20));
        }

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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Book model)
        {
            if (ModelState.IsValid)
            {
                var newBook = _service.AddBook(model);
                return RedirectToAction("ListBooks", "Book");
            }

            return View(model);
        }

        
        public IActionResult Delete(int id)
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

        [HttpPost]
        public IActionResult DeleteBook(Book book)
        {
            _service.DeleteBook(book.BookId);
            return RedirectToAction("ListBooks", "Book");
        }

        
        public IActionResult Update(int id)
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

        [HttpPost]
        public IActionResult Update(int id, Book model)
        {
            if (ModelState.IsValid)
            {
                var newBook = _service.UpdateBook(id, model);
                return RedirectToAction("ListBooks", "Book");
            }

            return View(model);
        }
    }
}
