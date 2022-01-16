using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FavoriteBook.Models
{
    public class Book
    {
        public int BookId { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        [Required]
        [StringLength(40)]
        public string Author { get; set; }
        [Required]
        public string Genre { get; set; }
        [Required]
        [Range(1, 1500)]
        public int Pages { get; set; }
        public int Published { get; set; }
        public string CoverUrl { get; set; }
        
        public ICollection<User> Users { get; set; }
        public ICollection<Membership> Memberships { get; set; }

       
    }
}
