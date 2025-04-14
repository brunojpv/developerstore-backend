namespace DeveloperStore.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public decimal Price { get; set; }
        public string Description { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string Image { get; set; } = null!;
        public Rating Rating { get; set; } = new();
    }

    public class Rating
    {
        public decimal Rate { get; set; }
        public int Count { get; set; }
    }
}
