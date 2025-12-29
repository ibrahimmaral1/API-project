using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Net9ApiOdev.Data.Repositories;
using BCrypt.Net;

namespace Net9ApiOdev.Service
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;

        public AuthService(IConfiguration config, IUserRepository userRepository)
        {
            _config = config;
            _userRepository = userRepository;
        }

        public string GenerateJwtToken(string username, string role, int userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            
            // JWT içinde tutulacak bilgiler (Rol ve Id dahil)
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30), // Token ömrü 30 dakika
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Giriş şifresini veritabanındaki hashlenmiş şifre ile karşılaştırır.
        public async Task<bool> ValidateUserCredentials(string email, string password)
        {
            // Kullanıcıyı e-posta ile bulmak için UserRepository'ye metot eklememiz gerek (Aşama 2.3)
            var user = await _userRepository.GetUserByEmailAsync(email); 

            if (user == null)
            {
                return false;
            }
            
            // BCrypt kullanarak şifreyi doğrula
            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }
    }
}