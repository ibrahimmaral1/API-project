using System.ComponentModel.DataAnnotations;

namespace Net9ApiOdev.DTOs
{
    public class UserUpdateDTO
    {
        // Güncellemede bu alanların zorunlu olması gerekmez, 
        // kullanıcı sadece birini güncellemek isteyebilir.
        [MaxLength(50)]
        public string? Username { get; set; }

        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi girin.")]
        public string? Email { get; set; }
        
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        public string? Password { get; set; }
    }
}