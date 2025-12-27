using Microsoft.EntityFrameworkCore;
using Net9ApiOdev.Models.Entities;

namespace Net9ApiOdev.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        // Diğer Entity'ler: Product, Order, Review vb. buraya eklenecek.
        // public DbSet<Product> Products { get; set; } 
        // public DbSet<Order> Orders { get; set; } 
        
        // ModelBuilder ile ilişkiler ve ek konfigürasyonlar buraya yapılır
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Örnek olarak User tablosuna indeks ekleyelim
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}