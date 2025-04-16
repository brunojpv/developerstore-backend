using DeveloperStore.Infrastructure.Seeders;

namespace DeveloperStore.Tests.Unit
{
    public class UserServiceTests
    {
        [Fact]
        public void CreateUser_ShouldContainValidEmail()
        {
            // Arrange
            var user = DevelopmentDataFactory.UserFaker.Generate();

            // Act & Assert
            Assert.Matches(".+@.+\\..+", user.Email);
        }

        [Fact]
        public void CreateUser_ShouldHaveFullName()
        {
            // Arrange
            var user = DevelopmentDataFactory.UserFaker.Generate();

            // Act
            var fullName = $"{user.Name.FirstName} {user.Name.LastName}";

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(fullName));
        }
    }
}
