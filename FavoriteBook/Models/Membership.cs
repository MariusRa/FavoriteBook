

namespace FavoriteBook.Models
{
    public class Membership
    {
        public User User { get; set; }
        public Book Book { get; set; }
        public bool IsBookRead { get; set; }
    }
}
