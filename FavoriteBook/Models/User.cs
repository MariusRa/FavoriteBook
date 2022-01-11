using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace FavoriteBook.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        
        public ICollection<Book> Books { get; set; }
    }
}
