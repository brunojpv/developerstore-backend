namespace DeveloperStore.Application.Events
{
    public class SaleModifiedEvent
    {
        public int SaleId { get; set; }
        public string ModifiedBy { get; set; } = string.Empty;
        public DateTime ModifiedAt { get; set; }
    }
}
