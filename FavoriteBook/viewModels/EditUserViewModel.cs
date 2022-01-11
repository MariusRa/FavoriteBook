using FavoriteBook.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FavoriteBook.viewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        
        [Required]
        public string UserName { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public List<string> Claims { get; set; }
        public IList<string> Roles { get; set; }
        public IEnumerable<Book> Books { get; set; }


        public EditUserViewModel()
        {
            Claims = new List<string>();
            Roles = new List<string>();
        }
    }




}
