using Domain.Entities.dbo.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Command.CreateUser
{
    internal class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, bool>
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public CreateUserCommandHandler(UserManager<User> userManager,
                                        RoleManager<Role> roleManager)
        {
            _userManager = userManager ?? throw new System.ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new System.ArgumentNullException(nameof(roleManager));
        }

        public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Age = request.Age,
                FullName = request.FullName,
                Gender = request.Gender,
                UserName = request.UserName,
                Email = request.Email
            };

            var createUserResult = await _userManager.CreateAsync(user, request.Password);

            var addRoleResult = await _roleManager.CreateAsync(new Role
            {
                Name = "Admin",
                Description = "admin role"
            });

            var assignRoleResult = await _userManager.AddToRoleAsync(user, "Admin");

            return true;
        }
    }
}
