using ArandaProduct.Domain.Entity;
using ArandaProduct.Infraestructure.Entity;
using Microsoft.EntityFrameworkCore;

namespace ArandaProduct.Infraestructure
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
