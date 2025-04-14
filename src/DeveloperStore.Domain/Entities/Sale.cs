namespace DeveloperStore.Domain.Entities
{
    public class Sale
    {
        public Guid Id { get; set; }
        public string SaleNumber { get; set; } = null!;
        public DateTime SaleDate { get; set; } = DateTime.UtcNow;
        public string Customer { get; set; } = null!;
        public string Branch { get; set; } = null!;
        public bool IsCancelled { get; private set; }
        public List<SaleItem> Items { get; set; } = new();

        public decimal TotalAmount => Items.Sum(i => i.Total);

        public void Cancel() => IsCancelled = true;
    }
}
