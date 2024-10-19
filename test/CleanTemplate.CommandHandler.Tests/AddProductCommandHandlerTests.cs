using CleanTemplate.Common.Exceptions;
using CleanTemplate.Persistence.CommandHandlers.Products;
using CleanTemplate.Persistence.Db;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CleanTemplate.CommandHandler.Tests
{
    public class AddProductCommandHandlerTests
    {
        [Fact]
        public async Task Should_ThrowException_When_InputIsNull()
        {
            var dbContext = new Mock<CleanArchWriteDbContext>();
            var logger = new Mock<ILogger<AddProductCommandHandler>>();

            var commandHandler = new AddProductCommandHandler(dbContext.Object, logger.Object);

            await Assert.ThrowsAsync<InvalidNullInputException>(() => commandHandler.Handle(null, CancellationToken.None));
        }
    }
}