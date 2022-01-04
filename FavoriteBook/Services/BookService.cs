using FavoriteBook.DAL;
using FavoriteBook.Models;
using System.Collections.Generic;
using System.Linq;

namespace FavoriteBook.Services
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _db;
        public BookService(ApplicationDbContext db)
        {
            _db = db;
        }

        

        public IEnumerable<Book> GetAllBooks(int page, int size)
        {
            return _db.Books.Skip((page - 1) * size).Take(size).ToList();
        }

        public IEnumerable<Book> GetBookByOwner(string id)
        {
            return _db.Books.Where(b => b.Owner.Id == id).ToList();
        }

        public Book GetById(int id, string ownerId)
        {
            return _db.Books.FirstOrDefault(x => x.BookId == id && x.Owner.Id == ownerId);
        }

        public Book AddBook(Book book, string ownerId)
        {
            var user = _db.Users.Where(u => u.Id == ownerId).SingleOrDefault();
            book.Owner = user;
            _db.Books.Add(book);
            _db.SaveChanges();
            return book;
        }

        public Book DeleteBook(int id, string ownerId)
        {
            var getBook = GetById(id, ownerId);
            _db.Books.Remove(getBook);
            _db.SaveChanges();
            return getBook;
        }

        public Book UpdateBook(int id, Book book)
        {
            _db.Books.Update(book);
            _db.SaveChanges();
            return book;
        }

        public Book IsRead(int id, bool value, string ownerId)
        {
            var book = GetById(id, ownerId);
            book.IsRead = value;
            _db.SaveChanges();
            return book;
        }
    }
}
