namespace Api.Controllers
{
    using ApiFramework.Attributes;
    using AutoMapper;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ValidateModelState]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BaseController : ControllerBase
    {
        internal readonly IMediator Mediator;
        internal readonly IMapper _mapper;
        internal readonly ILogger Logger;

        protected BaseController(ILogger logger,
                                 IMediator mediator,
                                 IMapper mapper)
        {
            Logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            Mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new System.ArgumentNullException(nameof(mapper));
        }
    }
}