using Microsoft.EntityFrameworkCore;

namespace LostButFound.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Lost> Losts { get; set; }
        public DbSet<Found> Founds { get; set; }
    }
}
