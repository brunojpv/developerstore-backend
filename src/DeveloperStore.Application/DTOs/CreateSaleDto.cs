﻿namespace DeveloperStore.Application.DTOs
{
    public class CreateSaleDto
    {
        public string SaleNumber { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int CustomerId { get; set; }
        public string Branch { get; set; } = string.Empty;
        public List<SaleItemDto> Items { get; set; } = new();
    }
}
