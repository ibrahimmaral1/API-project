using System.ComponentModel.DataAnnotations;

namespace Net9ApiOdev.DTOs
{
    public class UserCreateDTO
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        [MaxLength(50)]
        public string Username { get; set; }= string.Empty;

        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi girin.")]
        public string Email { get; set; }= string.Empty;

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        public string Password { get; set; } = string.Empty;
    }
}