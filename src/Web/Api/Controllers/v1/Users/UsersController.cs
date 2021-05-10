using Api.Controllers.v1.Users.Requests;
using ApiFramework.Tools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Controllers.v1.Users
{
    [ApiVersion("1")]
    public class UsersController : BaseController
    {
        public UsersController(ILogger<UsersController> logger,
                               IMediator mediator)
            : base(logger, mediator)
        { }

        [HttpPost("login")]
        [AllowAnonymous]
        public virtual async Task<ApiResult<bool>> SingUpAsync(SingUpRequest request, CancellationToken cancellationToken)
        {
            return new ApiResult<bool>(true);
        }

        [HttpPost("signup")]
        [AllowAnonymous]
        public virtual async Task<ApiResult<bool>> LoginAsync([FromForm] LoginRequest tokenRequest, CancellationToken cancellationToken)
        {
            return new ApiResult<bool>(true);

        }
    }
}
