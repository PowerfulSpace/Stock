using Microsoft.EntityFrameworkCore;
using Stock.Models;

namespace Stock.Data
{
    public class InventoryContext : DbContext
    {
        public InventoryContext(DbContextOptions<InventoryContext> options) :base(options)
        {
            
        }

        public DbSet<Unit> Units { get; set; } = null!;
    }
}
