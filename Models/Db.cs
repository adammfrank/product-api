using Microsoft.EntityFrameworkCore;

namespace ProductApi.Models
{
    public class Db(DbContextOptions<Db> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
    }
}