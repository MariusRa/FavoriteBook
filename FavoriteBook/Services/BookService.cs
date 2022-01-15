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

        // Gaunamos visos books iš Db
        public IEnumerable<Book> GetAllBooks(int page, int size)
        {
            return _db.Books.Skip((page - 1) * size).Take(size).ToList();
        }
              
       // Gaunama konkreti book pagal jos ID
        public Book GetBookById(int bookId)
        {
            return _db.Books.FirstOrDefault(x => x.BookId == bookId);
        }
                
        // Db išsaugoma nauja knyga
        public Book AddBook(Book book)
        {
            _db.Books.Add(book);
            _db.SaveChanges();
            return book;
        }

        // Iš Db ištrinama book
        public Book DeleteBook(int id)
        {
            var getBook = GetBookById(id);
            _db.Books.Remove(getBook);
            _db.SaveChanges();
            return getBook;
        }

        // DB atnaujinama info apie book
        public Book UpdateBook(int id, Book book)
        {
            _db.Books.Update(book);
            _db.SaveChanges();
            return book;
        }

        // Gaunamos visos Books pagal User ID iš Db
        public IEnumerable<Book> GetBookByOwner(string id)
        {
            return _db.Books.Where(u => u.Users.Any(i => i.Id == id)).ToList();
        }

        // Gaunama konkreti Users' book pagal book ID ir User ID
        public Book GetById(int id, string ownerId)
        {
            return _db.Books.FirstOrDefault(x => x.BookId == id && x.Users.Any(i => i.Id == ownerId));
        }

        // Db sukuriamas ryšys tarp Book ir User, kai User pasirinka norimą book.
        public Book AddBookUser(int bookId, string userId)
        {
            var book = _db.Books.Include(p => p.Users).Single(p => p.BookId == bookId);
            var user = _db.Users.Where(u => u.Id == userId).SingleOrDefault();

            book.Users.Add(user);
          
            _db.SaveChanges();

            return book;
        }
             
        // Panaikinamas ryšys tarp Book ir User
        public Book DeleteBookUser(int bookId, string userId)
        {
            var book = _db.Books.Include(p => p.Users).Single(p => p.BookId == bookId);
            var user = _db.Users.Where(u => u.Id == userId).SingleOrDefault();

            book.Users.Remove(user);
            _db.SaveChanges();

            return book;
        }  

        // Ryšių table atnaujinama info apie papildomą props'ą
        public Membership IsBookRead(int id, bool value, string userId)
        {
            var member = _db.Memberships.FirstOrDefault(x => x.Book.BookId == id && x.User.Id == userId);
            member.IsBookRead = value;

            _db.SaveChanges();
            
            return member ;
        }
    }
}
