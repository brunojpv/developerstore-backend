namespace DeveloperStore.Application.Events
{
    internal class SaleItemCancelledEvent
    {
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
