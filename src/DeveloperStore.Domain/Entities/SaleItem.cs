namespace DeveloperStore.Domain.Entities
{
    public class SaleItem
    {
        public Guid Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; private set; }

        public decimal Total => (UnitPrice * Quantity) - Discount;

        public void ApplyDiscount()
        {
            if (Quantity >= 10 && Quantity <= 20)
                Discount = (UnitPrice * Quantity) * 0.20m;
            else if (Quantity >= 4 && Quantity < 10)
                Discount = (UnitPrice * Quantity) * 0.10m;
            else
                Discount = 0;
        }
    }
}
