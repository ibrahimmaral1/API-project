
namespace Net9ApiOdev.Service
{
    // Kimlik doğrulama servis kontratı
    public interface IAuthService
    {
        // Token üretir (Login sonrası)
        string GenerateJwtToken(string username, string role, int userId);
        
        // Kullanıcı şifresini doğrular
        Task<bool> ValidateUserCredentials(string email, string password);
    }
}