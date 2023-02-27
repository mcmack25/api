using ip_test_api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ip_test_api.Data
{
    public class UsersContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; } = null!;

        public UsersContext(DbContextOptions options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsersConfiguration());
        }
    }
}
