using CactusProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CactusProject.Data
{
    public class CactusContext : IdentityDbContext
    {
        public CactusContext(DbContextOptions<CactusContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cactus> ManyCactus { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<Genus> ManyGenus { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<AddCactus> AddManyCactus { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ReviewImage> ReviewImages { get; set; }

    }
}
