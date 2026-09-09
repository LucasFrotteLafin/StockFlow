using Microsoft.EntityFrameworkCore;
using FocusSpace.Models;
using FocusSpace.Mappings;

namespace FocusSpace.DatabaseContext
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Movement> Movements { get; set; }
        public DbSet<UserRequest> UserRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Indice unico no SKU
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.SKU)
                .IsUnique();

            // Indice unico no Username de UserRequest
            modelBuilder.Entity<UserRequest>()
                .HasIndex(u => u.Username)
                .IsUnique()
                .HasDatabaseName("IX_UserRequest_Username");
        }
    }
}
