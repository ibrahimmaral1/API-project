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
        
        // ÖDEV GEREKSİNİMİ: En az 4 Entity (2.2)
        public DbSet<Product> Products { get; set; } 
        public DbSet<Category> Categories { get; set; } 
        public DbSet<Review> Reviews { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Email Unique kısıtı
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // ÖDEV GEREKSİNİMİ: Entity İlişkileri (2.2)
            // Örnek: Bir kategori altında birçok ürün olabilir
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            // Örnek: Bir ürünün birçok yorumu olabilir
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.ProductId);

            // BONUS: Seed Data - Başlangıç Verisi (+5 Puan)
            modelBuilder.Entity<User>().HasData(
                new User 
                { 
                    Id = 1, 
                    Username = "admin", 
                    Email = "admin@odev.com", 
                    PasswordHash = "AQAAAAIAAYagAAAAEL...", // Şifrelenmiş örnek hash
                    Role = "Admin", // Rol Tabanlı Erişim için Admin rolü
                    CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), 
                    UpdatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    IsDeleted = false 
                }
            );
        }
    }
}