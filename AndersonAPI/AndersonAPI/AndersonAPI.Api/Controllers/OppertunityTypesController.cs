using System.Net.Mime;
using AndersonAPI.Api.Controllers.ResponseTypes;
using AndersonAPI.Application.OppertunityTypes;
using AndersonAPI.Application.OppertunityTypes.CreateOppertunityType;
using AndersonAPI.Application.OppertunityTypes.DeleteOppertunityType;
using AndersonAPI.Application.OppertunityTypes.GetOppertunityTypeById;
using AndersonAPI.Application.OppertunityTypes.GetOppertunityTypes;
using AndersonAPI.Application.OppertunityTypes.UpdateOppertunityType;
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
    public class OppertunityTypesController : ControllerBase
    {
        private readonly ISender _mediator;

        public OppertunityTypesController(ISender mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// </summary>
        /// <response code="201">Successfully created.</response>
        /// <response code="400">One or more validation errors have occurred.</response>
        [HttpPost("api/oppertunity-types")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<Guid>>> CreateOppertunityType(
            [FromBody] CreateOppertunityTypeCommand command,
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetOppertunityTypeById), new { id = result }, new JsonResponse<Guid>(result));
        }

        /// <summary>
        /// </summary>
        /// <response code="200">Successfully deleted.</response>
        /// <response code="400">One or more validation errors have occurred.</response>
        /// <response code="404">One or more entities could not be found with the provided parameters.</response>
        [HttpDelete("api/oppertunity-types/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteOppertunityType(
            [FromRoute] Guid id,
            CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new DeleteOppertunityTypeCommand(id: id), cancellationToken);
            return Ok();
        }

        /// <summary>
        /// </summary>
        /// <response code="204">Successfully updated.</response>
        /// <response code="400">One or more validation errors have occurred.</response>
        /// <response code="404">One or more entities could not be found with the provided parameters.</response>
        [HttpPut("api/oppertunity-types/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateOppertunityType(
            [FromRoute] Guid id,
            [FromBody] UpdateOppertunityTypeCommand command,
            CancellationToken cancellationToken = default)
        {
            if (command.Id == Guid.Empty)
            {
                command.Id = id;
            }

            if (id != command.Id)
            {
                return BadRequest();
            }

            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// </summary>
        /// <response code="200">Returns the specified OppertunityTypeDto.</response>
        /// <response code="400">One or more validation errors have occurred.</response>
        /// <response code="404">No OppertunityTypeDto could be found with the provided parameters.</response>
        [HttpGet("api/oppertunity-types/{id}")]
        [ProducesResponseType(typeof(OppertunityTypeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<OppertunityTypeDto>> GetOppertunityTypeById(
            [FromRoute] Guid id,
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetOppertunityTypeByIdQuery(id: id), cancellationToken);
            return result == null ? NotFound() : Ok(result);
        }

        /// <summary>
        /// </summary>
        /// <response code="200">Returns the specified List&lt;OppertunityTypeDto&gt;.</response>
        [HttpGet("api/oppertunity-types")]
        [ProducesResponseType(typeof(List<OppertunityTypeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<OppertunityTypeDto>>> GetOppertunityTypes(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetOppertunityTypesQuery(), cancellationToken);
            return Ok(result);
        }
    }
}