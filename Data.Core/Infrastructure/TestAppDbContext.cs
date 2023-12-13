using Data.Core.Model;
using Microsoft.EntityFrameworkCore;

namespace Data.Core.Infrastructure
{
    public class TestAppDbContext : DbContext
    {
        public TestAppDbContext(DbContextOptions<TestAppDbContext> options) : base(options)
        {
        }
        public DbSet<Personal?> Personals { get; set; }
    }
}
