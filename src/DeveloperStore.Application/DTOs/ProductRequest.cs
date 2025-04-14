namespace DeveloperStore.Application.DTOs
{
    public class ProductRequest
    {
        public string Title { get; set; } = null!;
        public decimal Price { get; set; }
        public string Description { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string Image { get; set; } = null!;
        public RatingDto Rating { get; set; } = new();
    }

    public class RatingDto
    {
        public decimal Rate { get; set; }
        public int Count { get; set; }
    }
}
