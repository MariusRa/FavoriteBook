using FavoriteBook.DAL;
using FavoriteBook.Models;
using FavoriteBook.viewModels;
using Microsoft.EntityFrameworkCore;
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
            return _db.Books.Where(u => u.Users.Any(i => i.Id == id)).ToList();
            //return _db.Books.Where(b => b.Owner.Id == id).ToList();
        }

        public Book GetBookById(int bookId)
        {
            return _db.Books.FirstOrDefault(x => x.BookId == bookId);
        }

        public Book GetById(int id, string ownerId)
        {
            return _db.Books.FirstOrDefault(x => x.BookId == id && x.Users.Any(i => i.Id == ownerId));
        }

        public Book AddBook(Book book/*, string ownerId*/)
        {
            //var user = _db.Users.Where(u => u.Id == ownerId).SingleOrDefault();
            //book.Users.Add(user);
            _db.Books.Add(book);
            _db.SaveChanges();
            return book;
        }

        public Book AddBookUser(int bookId, string userId)
        {
            var book = _db.Books.Include(p => p.Users).Single(p => p.BookId == bookId);
            var user = _db.Users.Where(u => u.Id == userId).SingleOrDefault();

            book.Users.Add(user);
          
            _db.SaveChanges();

            return book;
        }

        public Book DeleteBook(int id/*, string ownerId*/)
        {
            var getBook = GetBookById(id/*, ownerId*/);
            _db.Books.Remove(getBook);
            _db.SaveChanges();
            return getBook;
        }

        public Book DeleteBookUser(int bookId, string userId)
        {
            var book = _db.Books.Include(p => p.Users).Single(p => p.BookId == bookId);
            var user = _db.Users.Where(u => u.Id == userId).SingleOrDefault();

            book.Users.Remove(user);
            _db.SaveChanges();

            return book;
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

        public Membership IsBookRead(int id, bool value, string userId)
        {
            //var existingBook = _db.Books.Include(p => p.Users).Single(p => p.BookId == id);
            //var existingUser = _db.Users.Where(u => u.Id == userId).SingleOrDefault();

            //var model = new Membership
            //{
            //   Book = existingBook,
            //   User = existingUser,
            //   IsBookRead = value
            //};

            //existingBook.Users.Remove(existingUser);

            var member = _db.Memberships.FirstOrDefault(x => x.Book.BookId == id && x.User.Id == userId);
            member.IsBookRead = value;

            _db.SaveChanges();
            
            return member ;
        }
    }
}
