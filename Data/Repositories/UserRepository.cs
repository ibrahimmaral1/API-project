using Microsoft.EntityFrameworkCore;
using Net9ApiOdev.Models.Entities;

namespace Net9ApiOdev.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Soft Delete: Sadece silinmemiş kullanıcıları getir 
        /// </summary>
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .Where(u => !u.IsDeleted) // Silinmişleri filtrele
                .ToListAsync();
        }

        // Soft Delete: Sadece silinmemişse getir
        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
        }

        public async Task<User> AddAsync(User user)
        {
            user.CreatedAt = DateTime.UtcNow; // Gereksinim 2.2 
            user.UpdatedAt = DateTime.UtcNow; // Gereksinim 2.2 [cite: 15]
            user.IsDeleted = false; // Başlangıçta silinmemiş [cite: 81]  
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task UpdateAsync(User user)
        {
            user.UpdatedAt = DateTime.UtcNow; // UpdatedAt alanını güncelleme gereksinimi [cite: 15]
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        // FİZİKSEL SİLME YERİNE SOFT DELETE [cite: 80, 81]
        public async Task DeleteAsync(User user)
        {
            user.IsDeleted = true; 
            user.UpdatedAt = DateTime.UtcNow; 
            
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Users.AnyAsync(u => u.Id == id && !u.IsDeleted);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
        }
    }
}