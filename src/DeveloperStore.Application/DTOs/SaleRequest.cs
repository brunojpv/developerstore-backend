namespace DeveloperStore.Application.DTOs
{
    public class SaleRequest
    {
        public string Customer { get; set; } = null!;
        public string Branch { get; set; } = null!;
        public List<SaleItemRequest> Items { get; set; } = new();
    }

    public class SaleItemRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
