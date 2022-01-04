using FavoriteBook.DAL;
using FavoriteBook.Models;

namespace FavoriteBook.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _db;
        public UserService(ApplicationDbContext db)
        {
            _db = db;
        }
        public User AddUser(User user)
        {
            _db.Users.Add(user);
            _db.SaveChanges();
            
            return user;
        }
    }
}
