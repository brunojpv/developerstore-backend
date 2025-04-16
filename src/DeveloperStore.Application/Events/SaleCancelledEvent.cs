namespace DeveloperStore.Application.Events
{
    public class SaleCancelledEvent
    {
        public int SaleId { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}
