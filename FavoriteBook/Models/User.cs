using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace FavoriteBook.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public List<Book> MyBooks { get; set; }
    }
}
