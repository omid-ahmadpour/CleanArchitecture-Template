namespace CleanTemplate.Api.Controllers.v1
{
    using ApiFramework.Attributes;
    using AutoMapper;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ValidateModelState]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BaseControllerV1 : ControllerBase
    {
        internal readonly IMediator _mediator;
        internal readonly IMapper _mapper;
        internal readonly ILogger _logger;

        protected BaseControllerV1(ILogger logger,
                                   IMediator mediator,
                                   IMapper mapper)
        {
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new System.ArgumentNullException(nameof(mapper));
        }
    }
}