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
        Book AddBook(Book book);
        Book DeleteBook(int id);
        Book UpdateBook(int id, Book book);
        Book AddBookUser(int bookId, string userId);
        Book DeleteBookUser(int bookId, string userId);

        Membership IsBookRead(int id, bool value, string userId);
    }
}
