using AutoMapper;
using DeveloperStore.Application.Mappings;
using DeveloperStore.Application.UseCases.Users;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Enum;
using DeveloperStore.Domain.Interfaces;
using NSubstitute;

namespace DeveloperStore.Tests.Unit
{
    public class GetAllUsersHandlerTests
    {
        [Fact]
        public async Task Handle_Returns_Users_As_UserDto()
        {
            var mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
            var userRepo = Substitute.For<IUserRepository>();
            userRepo.GetAllAsync().Returns(new List<User> {
            new() { Id = 1, Username = "admin", Email = "admin@email.com", Role = UserRole.Admin, Status = UserStatus.Active }
        });

            var handler = new GetAllUsersHandler(userRepo, mapper);
            var result = await handler.Handle(new GetAllUsersQuery(), default);

            Assert.Single(result);
            Assert.Equal("admin", result.First().Username);
        }
    }
}
