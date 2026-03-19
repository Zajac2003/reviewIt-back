using Microsoft.EntityFrameworkCore;
using review_microservice.Models;

namespace review_microservice.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Review> Reviews { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
