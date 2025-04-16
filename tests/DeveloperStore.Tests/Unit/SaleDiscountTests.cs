namespace DeveloperStore.Tests.Unit
{
    public class SaleDiscountTests
    {
        private decimal CalculateDiscount(int quantity, decimal unitPrice)
        {
            if (quantity > 20) throw new ArgumentException("Cannot sell more than 20 identical items");
            if (quantity >= 10) return 0.20m * quantity * unitPrice;
            if (quantity >= 4) return 0.10m * quantity * unitPrice;
            return 0m;
        }

        [Theory]
        [InlineData(3, 100, 0)]
        [InlineData(4, 100, 40)]
        [InlineData(10, 100, 200)]
        [InlineData(20, 100, 400)]
        public void CalculateDiscount_ShouldReturnCorrectAmount(int qty, decimal price, decimal expectedDiscount)
        {
            // Act
            var discount = CalculateDiscount(qty, price);

            // Assert
            Assert.Equal(expectedDiscount, discount);
        }

        [Fact]
        public void CalculateDiscount_ShouldThrow_WhenQuantityExceedsLimit()
        {
            Assert.Throws<ArgumentException>(() => CalculateDiscount(21, 100));
        }
    }
}
