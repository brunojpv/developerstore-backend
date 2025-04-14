namespace DeveloperStore.Application.DTOs
{
    public class SaleResponse
    {
        public Guid Id { get; set; }
        public string SaleNumber { get; set; } = null!;
        public DateTime SaleDate { get; set; }
        public string Customer { get; set; } = null!;
        public string Branch { get; set; } = null!;
        public bool IsCancelled { get; set; }
        public decimal TotalAmount { get; set; }
        public List<SaleItemResponse> Items { get; set; } = new();
    }

    public class SaleItemResponse
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
    }
}
