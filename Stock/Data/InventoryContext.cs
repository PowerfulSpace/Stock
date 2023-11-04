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
    }
}
