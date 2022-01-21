using CleanTemplate.Common.Exceptions;
using CleanTemplate.Domain.Entities.Users;
using CleanTemplate.Persistance.CommandHandlers.Users;
using CleanTemplate.Persistance.Jwt;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CleanTemplate.CommandHandler.Test
{
    public class LoginCommandHandlerTest
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

            var userManager = new UserManager<User>(userStore.Object, null, null, null, null, null, null, null, null);
            var jwtService = new Mock<IJwtService>();

            // Act
            var commandHandler = new LoginCommandHandler(userManager, jwtService.Object);

            // Assert
            await Assert.ThrowsAsync<InvalidNullInputException>(() => commandHandler.Handle(null, CancellationToken.None));
        }
    }
}
