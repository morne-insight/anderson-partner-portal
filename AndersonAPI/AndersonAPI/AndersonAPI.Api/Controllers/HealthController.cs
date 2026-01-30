using System.Net.Mime;
using AndersonAPI.Api.Controllers.ResponseTypes;
using AndersonAPI.Application.Health.GetApiHealth;
using AndersonAPI.Application.Health.GetAuthHealth;
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
        /// Hits the database via CQRS pipeline without authentication
        /// </summary>
        /// <response code="200">Returns the specified string.</response>
        [HttpGet("api/health/api")]
        [AllowAnonymous]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<string>>> GetApiHealth(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetApiHealthQuery(), cancellationToken);
            return Ok(new JsonResponse<string>(result));
        }

        /// <summary>
        /// Hits the database via CQRS pipeline with authentication
        /// </summary>
        /// <response code="200">Returns the specified string.</response>
        /// <response code="401">Unauthorized request.</response>
        /// <response code="403">Forbidden request.</response>
        [HttpGet("api/health/auth")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<string>>> GetAuthHealth(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAuthHealthQuery(), cancellationToken);
            return Ok(new JsonResponse<string>(result));
        }

        [HttpGet("api/health")]
        [AllowAnonymous]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [IntentManaged(Mode.Ignore)]
        public async Task<ActionResult<JsonResponse<string>>> GetRoot(CancellationToken cancellationToken = default)
        {
            return Ok(new JsonResponse<string>("Alive"));
        }
    }
}