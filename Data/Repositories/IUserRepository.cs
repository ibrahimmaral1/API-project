using Net9ApiOdev.Models.Entities;

namespace Net9ApiOdev.Data.Repositories
{
    // Veritabanı erişim kontratı (AOI/Interface Prensibi)
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User> AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task<bool> ExistsAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);
    }
}