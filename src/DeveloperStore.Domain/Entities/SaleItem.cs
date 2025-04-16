namespace DeveloperStore.Domain.Entities
{
    public class SaleItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public bool Cancelled { get; set; }

        public decimal Discount => Quantity switch
        {
            >= 10 and <= 20 => 0.20m,
            >= 4 and < 10 => 0.10m,
            > 20 => throw new InvalidOperationException("Maximum quantity per product is 20."),
            _ => 0m
        };

        public decimal Total => Quantity * UnitPrice * (1 - Discount);
    }
}
