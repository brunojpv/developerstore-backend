namespace DeveloperStore.Application.DTOs
{
    public class CartRequest
    {
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public List<CartItemRequest> Products { get; set; } = new();
    }

    public class CartItemRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
