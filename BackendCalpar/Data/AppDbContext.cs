using Microsoft.EntityFrameworkCore;
using BackendCalpar.Models;

namespace BackendCalpar.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
    }
}
