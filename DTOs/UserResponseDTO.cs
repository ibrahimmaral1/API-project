namespace Net9ApiOdev.DTOs
{
    // API'nin dış dünyaya döndüreceği format
    public class UserResponseDTO
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // İlişkili DTO'lar buraya eklenebilir (Örn: List<OrderResponseDTO> Orders)
    }
}