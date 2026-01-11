namespace Net9ApiOdev.Models.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public string Comment { get; set; } = string.Empty;
        public int Rating { get; set; }
        public int ProductId { get; set; } // Foreign Key
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; 

        // İlişki 
        public Product Product { get; set; } = null!;
    }
}