using AutoMapper;
using CleanTemplate.Api.Controllers.v1.Users.Requests;
using CleanTemplate.ApiFramework.Tools;
using CleanTemplate.Application.Users.Command.CreateUser;
using CleanTemplate.Application.Users.Command.Login;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace CleanTemplate.Api.Controllers.v1.Users
{
    [ApiVersion("1")]
    public class UserController : BaseControllerV1
    {
        public UserController(ILogger<UserController> logger,
                              IMediator mediator,
                              IMapper mapper)
            : base(logger, mediator, mapper)
        { }

        [HttpPost("signup")]
        [SwaggerOperation("sign up user")]
        [AllowAnonymous]
        public virtual async Task<ApiResult<bool>> SingUpAsync(SingUpRequest request, CancellationToken cancellationToken)
        {
            var command = _mapper.Map<SingUpRequest, CreateUserCommand>(request);

            var result = await _mediator.Send(command, cancellationToken);
            return new ApiResult<bool>(result);
        }

        [HttpPost("login")]
        [SwaggerOperation("login by username and password")]
        [AllowAnonymous]
        public virtual async Task<ApiResult<LoginResponse>> LoginAsync([FromForm] LoginRequest request, CancellationToken cancellationToken)
        {
            var command = _mapper.Map<LoginRequest, LoginCommand>(request);

            var result = await _mediator.Send(command, cancellationToken);
            return new ApiResult<LoginResponse>(result);
        }
    }
}
