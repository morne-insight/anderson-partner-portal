using System.Net.Mime;
using AndersonAPI.Api.Controllers.ResponseTypes;
using AndersonAPI.Application.Health.GetHealth;
using Intent.RoslynWeaver.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: DefaultIntentManaged(Mode.Fully, Targets = Targets.Usings)]
[assembly: IntentTemplate("Intent.AspNetCore.Controllers.Controller", Version = "1.0")]

namespace AndersonAPI.Api.Controllers
{
    [ApiController]
    [Authorize]
    public class HealthController : ControllerBase
    {
        private readonly ISender _mediator;

        public HealthController(ISender mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// </summary>
        /// <response code="200">Returns the specified string.</response>
        [HttpGet("api/health")]
        [AllowAnonymous]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<ActionResult<JsonResponse<string>>> GetHealth(CancellationToken cancellationToken = default)
        {
            //var result = await _mediator.Send(new GetHealth(), cancellationToken);
            //return Ok(new JsonResponse<string>(result));
            return Ok(new JsonResponse<string>("Healty"));
        }

        [HttpGet("/")]
        [AllowAnonymous]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<ActionResult<JsonResponse<string>>> GetRoot(CancellationToken cancellationToken = default)
        {
            //var result = await _mediator.Send(new GetHealth(), cancellationToken);
            //return Ok(new JsonResponse<string>(result));
            return Ok(new JsonResponse<string>("Alive"));
        }
    }
}