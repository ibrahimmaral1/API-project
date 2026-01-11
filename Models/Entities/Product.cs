namespace Net9ApiOdev.Models.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int CategoryId { get; set; } // Foreign Key
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // [cite: 15]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; // [cite: 15]

        // İlişkiler [cite: 16]
        public Category Category { get; set; } = null!;
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}