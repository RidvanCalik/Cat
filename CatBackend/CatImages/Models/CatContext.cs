using Microsoft.EntityFrameworkCore;

namespace CatImages.Models
{
    public class CatContext : DbContext
    {
        public CatContext(DbContextOptions<CatContext> options) : base(options)
        {
        }

        public DbSet<CatItem> CatItems { get; set; } = null!;
    }
}
