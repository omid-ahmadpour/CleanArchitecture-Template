using Api.Controllers.v1.Users.Requests;
using ApiFramework.Tools;
using Application.Users.Command.CreateUser;
using Application.Users.Command.Login;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Controllers.v1.Users
{
    [ApiVersion("1")]
    public class UserController : BaseController
    {
        public UserController(ILogger<UserController> logger,
                               IMediator mediator,
                               IMapper mapper)
            : base(logger, mediator, mapper)
        { }

        [HttpPost("signup")]
        [AllowAnonymous]
        public virtual async Task<ApiResult<bool>> SingUpAsync(SingUpRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateUserCommand
            {
                Age = request.Age,
                Email = request.Email,
                FullName = request.FullName,
                Gender = request.Gender,
                Password = request.Password,
                UserName = request.UserName
            };

            var result = await Mediator.Send(command);
            return new ApiResult<bool>(result);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public virtual async Task<ApiResult<LoginResponse>> LoginAsync([FromForm] LoginRequest request, CancellationToken cancellationToken)
        {
            var command = new LoginCommand
            {
                Username = request.Username,
                Password = request.Password,
                Refresh_token = request.Refresh_token
            };

            var result = await Mediator.Send(command);
            return new ApiResult<LoginResponse>(result);
        }
    }
}
