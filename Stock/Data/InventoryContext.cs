using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stock.Models;

namespace Stock.Data
{
    public class InventoryContext : IdentityDbContext<IdentityUser>
    {
        public InventoryContext(DbContextOptions<InventoryContext> options) :base(options)
        {
            
        }

        public DbSet<Unit> Units { get; set; } = null!;
        public DbSet<ProductGroup> ProductGroups { get; set; } = null!;
        public DbSet<ProductProfile> ProductProfiles { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Brand> Brands { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
    }
}
