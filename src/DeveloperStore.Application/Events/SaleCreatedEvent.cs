namespace DeveloperStore.Application.Events
{
    public class SaleCreatedEvent
    {
        public int SaleId { get; set; }
        public string SaleNumber { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }
}
