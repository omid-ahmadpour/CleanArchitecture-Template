using CleanTemplate.Common.Exceptions;
using CleanTemplate.Domain.Entities.Users;
using CleanTemplate.Persistence.CommandHandlers.Users;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CleanTemplate.CommandHandler.Tests
{
    public class CreateUserCommandHandlerTests
    {
        [Fact]
        public async Task Should_ThrowException_When_InputIsNull()
        {
            // Arrange
            var userStore = new Mock<IUserStore<User>>();
            userStore.Setup(x => x.FindByIdAsync("123", CancellationToken.None))
                .ReturnsAsync(new User()
                {
                    UserName = "testUserName",
                    Id = 123
                });

            var roleStore = new Mock<IRoleStore<Role>>();
            roleStore.Setup(x => x.FindByIdAsync("123", CancellationToken.None))
                .ReturnsAsync(new Role()
                {
                    Name = "testRole",
                    Id = 123,
                });

            var userManager = TestHelpers.CreateMockUserManager(userStore.Object);
            var roleManager = TestHelpers.CreateMockRoleManager(roleStore.Object);

            var commandHandler = new CreateUserCommandHandler(userManager, roleManager);

            await Assert.ThrowsAsync<InvalidNullInputException>(() => commandHandler.Handle(null, CancellationToken.None));
        }
    }
}
