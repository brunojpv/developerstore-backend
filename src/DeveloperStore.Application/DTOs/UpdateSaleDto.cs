namespace DeveloperStore.Application.DTOs
{
    public class UpdateSaleDto
    {
        public int Id { get; set; }
        public string SaleNumber { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int CustomerId { get; set; }
        public string Branch { get; set; } = string.Empty;
        public bool Cancelled { get; set; }
        public List<SaleItemDto> Items { get; set; } = new();
    }
}
