using FavoriteBook.Models;
using System.Collections.Generic;


namespace FavoriteBook.viewModels
{
    public class BookMembershipViewModel
    {
        public int BookId { get; set; }
        
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public bool IsBookRead { get; set; }
        public string UserId { get; set; }
    }
}
