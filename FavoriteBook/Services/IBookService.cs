using FavoriteBook.Models;
using FavoriteBook.viewModels;
using System.Collections.Generic;

namespace FavoriteBook.Services
{
    public interface IBookService
    {
        IEnumerable<Book> GetAllBooks(int page, int size);
        Book GetBookById(int bookId);
        Book GetById(int id, string ownerId);
        IEnumerable<Book> GetBookByOwner(string id);
        Book AddBook(Book book/*, string ownerId*/);
        Book DeleteBook(int id/*, string ownerId*/);
        Book UpdateBook(int id, Book book);
        Book AddBookUser(int bookId, string userId);
        Book DeleteBookUser(int bookId, string userId);
        
        Book IsRead(int id, bool value, string ownerId);
    }
}
