using Domain.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Domain.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {

        public AppDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        
        }

        public DbSet<Category> Category { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<ShopCart> ShopCart { get; set; }
        public DbSet<ShopCartItems> ShopCartItems { get; set; }
        public DbSet<Order> Order { get; set; }
        
    }
}
