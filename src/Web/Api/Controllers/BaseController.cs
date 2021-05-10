namespace Api.Controllers
{
    using ApiFramework.Attributes;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ValidateModelState]
    [Route("api/v{version:apiVersion}/[controller]")]
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