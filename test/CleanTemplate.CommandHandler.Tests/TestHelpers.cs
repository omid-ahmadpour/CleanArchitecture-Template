using CleanTemplate.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;

namespace CleanTemplate.CommandHandler.Tests
{
    public static class TestHelpers
    {
        public static UserManager<User> CreateMockUserManager(IUserStore<User> userStore)
        {
            var passwordHasher = new Mock<IPasswordHasher<User>>();
            var userValidators = new[] { new Mock<IUserValidator<User>>().Object };
            var passwordValidators = new[] { new Mock<IPasswordValidator<User>>().Object };
            var keyNormalizer = new Mock<ILookupNormalizer>();
            var options = new Mock<IOptions<IdentityOptions>>();
            var identityErrorDescriber = new IdentityErrorDescriber();
            var serviceProvider = new Mock<IServiceProvider>();
            var logger = new Mock<ILogger<UserManager<User>>>();

            return new UserManager<User>(
                userStore,
                options.Object,
                passwordHasher.Object,
                userValidators,
                passwordValidators,
                keyNormalizer.Object,
                identityErrorDescriber,
                serviceProvider.Object,
                logger.Object);
        }

        public static RoleManager<Role> CreateMockRoleManager(IRoleStore<Role> roleStore)
        {
            var roleValidators = new[] { new Mock<IRoleValidator<Role>>().Object };
            var keyNormalizer = new Mock<ILookupNormalizer>();
            var identityErrorDescriber = new IdentityErrorDescriber();
            var logger = new Mock<ILogger<RoleManager<Role>>>();

            return new RoleManager<Role>(
                roleStore,
                roleValidators,
                keyNormalizer.Object,
                identityErrorDescriber,
                logger.Object);
        }
    }
}
