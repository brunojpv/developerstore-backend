﻿namespace DeveloperStore.Application.DTOs
{
    public class SaleItemResponseDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Total => (Quantity * UnitPrice) - Discount;
    }
}
