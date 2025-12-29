using Net9ApiOdev.DTOs;
using Net9ApiOdev.Models.Entities;

namespace Net9ApiOdev.Service
{
    // İş Mantığı Kontratı
    public interface IUserService
    {
        // CRUD metotları DTO'larla çalışmalı
        Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync();
        Task<UserResponseDTO?> GetUserByIdAsync(int id);
        Task<UserResponseDTO> CreateUserAsync(UserCreateDTO userDto);
        Task<bool> UpdateUserAsync(int id, UserUpdateDTO userDto);
        Task<bool> DeleteUserAsync(int id);
    }
}