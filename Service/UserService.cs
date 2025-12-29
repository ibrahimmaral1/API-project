using Net9ApiOdev.Data.Repositories;
using Net9ApiOdev.DTOs;
using Net9ApiOdev.Models.Entities;

namespace Net9ApiOdev.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            // Bağımlılık Enjeksiyonu ile Repository'yi alıyoruz
            _userRepository = userRepository;
        }

        // --- Entity'den Response DTO'ya Dönüşüm (Mapping) ---
        // (Normalde AutoMapper gibi kütüphaneler kullanılır, burada manuel yapıyoruz.)
        private UserResponseDTO MapToResponseDto(User user)
        {
            return new UserResponseDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }
        
        // --- CRUD Uygulamaları ---

        public async Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            // Entity listesini DTO listesine dönüştür ve döndür
            return users.Select(MapToResponseDto).ToList();
        }

        public async Task<UserResponseDTO?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user == null ? null : MapToResponseDto(user);
        }

        public async Task<UserResponseDTO> CreateUserAsync(UserCreateDTO userDto)
        {
            // DTO'dan Entity'ye Dönüşüm
            var newUser = new User
            {
                Username = userDto.Username,
                Email = userDto.Email,
                // NOT: Gerçek bir uygulamada şifre burada hashlenmelidir.
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                 Role = "User"
            };

            var createdUser = await _userRepository.AddAsync(newUser);
            
            // Entity'den Response DTO'ya Dönüşüm ve döndürme
            return MapToResponseDto(createdUser);
        }

        public async Task<bool> UpdateUserAsync(int id, UserUpdateDTO userDto)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null)
            {
                return false; // Kayıt bulunamadı
            }

            // Güncelleme: Yalnızca DTO'da gelen alanları güncelleriz
            if (!string.IsNullOrEmpty(userDto.Username))
            {
                existingUser.Username = userDto.Username;
            }

            if (!string.IsNullOrEmpty(userDto.Email))
            {
                existingUser.Email = userDto.Email;
            }

            if (!string.IsNullOrEmpty(userDto.Password))
            {
                existingUser.PasswordHash = userDto.Password;
            }
            
            await _userRepository.UpdateAsync(existingUser);
            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return false;
            }
            // Ödevin bonus gereksinimi olan Soft Delete (IsDeleted = true) de burada yapılabilir.
            await _userRepository.DeleteAsync(user);
            return true;
        }
    }
}