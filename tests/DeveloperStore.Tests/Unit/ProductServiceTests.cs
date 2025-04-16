using DeveloperStore.Infrastructure.Seeders;

namespace DeveloperStore.Tests.Unit
{
    public class ProductServiceTests
    {
        [Fact]
        public void Product_ShouldHaveValidPrice()
        {
            // Arrange
            var product = DevelopmentDataFactory.ProductFaker.Generate();

            // Assert
            Assert.True(product.Price > 0);
        }

        [Fact]
        public void Product_ShouldHaveRatingBetween1And5()
        {
            // Arrange
            var product = DevelopmentDataFactory.ProductFaker.Generate();

            // Assert
            Assert.InRange(product.Rating.Rate, 1, 5);
        }
    }
}
