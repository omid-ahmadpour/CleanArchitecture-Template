namespace Api.Controllers.v1
{
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("api/v{v:apiVersion}/{controller}")]
    public class BaseController : ControllerBase
    {
        internal readonly IMediator Mediator;
        internal readonly ILogger Logger;

        protected BaseController(ILogger logger, IMediator mediator)
        {
            Logger = logger;
            Mediator = mediator;
        }
    }
}