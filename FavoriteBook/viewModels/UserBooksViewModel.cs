using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FavoriteBook.viewModels
{
    public class UserBooksViewModel
    {
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public string BookAuthor { get; set; }
        public string BookGenre { get; set; }
        public bool IsSelected { get; set; }
    }
}
