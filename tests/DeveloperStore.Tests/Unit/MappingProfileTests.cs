using AutoMapper;
using DeveloperStore.Application.Mappings;

namespace DeveloperStore.Tests.Unit
{
    public class MappingProfileTests
    {
        [Fact]
        public void MappingProfile_Configuration_IsValid()
        {
            // Arrange
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());

            // Act & Assert
            config.AssertConfigurationIsValid();
        }
    }
}
