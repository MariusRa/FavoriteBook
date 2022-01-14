using FavoriteBook.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FavoriteBook.DAL
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Membership> Memberships { get; set; }

        public string DbPath { get; private set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer("Data Source=DESKTOP-QJFRKQB; Initial Catalog=DomainTask; Integrated security=true;");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasMany(u => u.Books)
                .WithMany(b => b.Users)
                .UsingEntity<Membership>(
                j => j.HasOne(m => m.Book).WithMany(g => g.Memberships),
                j => j.HasOne(m => m.User).WithMany(g => g.Memberships));
            base.OnModelCreating(builder);
        }
    }
}
