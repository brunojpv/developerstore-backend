namespace DeveloperStore.Application.DTOs
{
    public class SaleResponseDto
    {
        public string SaleNumber { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public decimal TotalAmount { get; set; }
        public List<SaleItemResultDto> Items { get; set; } = new();
    }
}
