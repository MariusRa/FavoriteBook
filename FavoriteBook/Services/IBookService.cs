

using FavoriteBook.Models;
using System.Collections.Generic;

namespace FavoriteBook.Services
{
    public interface IBookService
    {
        IEnumerable<Book> GetAllBooks(int page, int size);
        Book GetById(int id, string ownerId);
        IEnumerable<Book> GetBookByOwner(string id);
        Book AddBook(Book book, string ownerId);
        Book DeleteBook(int id, string ownerId);
        Book UpdateBook(int id, Book book);
        Book IsRead(int id, bool value, string ownerId);
    }
}
