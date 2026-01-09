using System.Net.Mime;
using AndersonAPI.Api.Controllers.ResponseTypes;
using AndersonAPI.Application.CompanyProfiles;
using AndersonAPI.Application.CompanyProfiles.CreateCompanyProfile;
using AndersonAPI.Application.CompanyProfiles.DeleteCompanyProfile;
using AndersonAPI.Application.CompanyProfiles.GetCompanyNames;
using AndersonAPI.Application.CompanyProfiles.GetCompanyProfileById;
using AndersonAPI.Application.CompanyProfiles.GetCompanyProfiles;
using AndersonAPI.Application.CompanyProfiles.UpdateProfileCompanyProfile;
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
    public class CompanyProfilesController : ControllerBase
    {
        private readonly ISender _mediator;

        public CompanyProfilesController(ISender mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// </summary>
        /// <response code="201">Successfully created.</response>
        /// <response code="400">One or more validation errors have occurred.</response>
        [HttpPost("api/company-profiles")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(JsonResponse<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<JsonResponse<Guid>>> CreateCompanyProfile(
            [FromBody] CreateCompanyProfileCommand command,
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetCompanyProfileById), new { id = result }, new JsonResponse<Guid>(result));
        }

        /// <summary>
        /// </summary>
        /// <response code="200">Successfully deleted.</response>
        /// <response code="400">One or more validation errors have occurred.</response>
        /// <response code="404">One or more entities could not be found with the provided parameters.</response>
        [HttpDelete("api/company-profiles/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteCompanyProfile(
            [FromRoute] Guid id,
            CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new DeleteCompanyProfileCommand(id: id), cancellationToken);
            return Ok();
        }

        /// <summary>
        /// </summary>
        /// <response code="204">Successfully updated.</response>
        /// <response code="400">One or more validation errors have occurred.</response>
        /// <response code="404">One or more entities could not be found with the provided parameters.</response>
        [HttpPut("api/company-profiles/{id}/profile")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateProfileCompanyProfile(
            [FromRoute] Guid id,
            [FromBody] UpdateProfileCompanyProfileCommand command,
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
        /// <response code="200">Returns the specified List&lt;CompanyNameDto&gt;.</response>
        [HttpGet("api/company-profiles/get-company-names")]
        [ProducesResponseType(typeof(List<CompanyNameDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<CompanyNameDto>>> GetCompanyNames(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetCompanyNamesQuery(), cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// </summary>
        /// <response code="200">Returns the specified CompanyProfileDto.</response>
        /// <response code="400">One or more validation errors have occurred.</response>
        /// <response code="404">No CompanyProfileDto could be found with the provided parameters.</response>
        [HttpGet("api/company-profiles/{id}")]
        [ProducesResponseType(typeof(CompanyProfileDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CompanyProfileDto>> GetCompanyProfileById(
            [FromRoute] Guid id,
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetCompanyProfileByIdQuery(id: id), cancellationToken);
            return result == null ? NotFound() : Ok(result);
        }

        /// <summary>
        /// </summary>
        /// <response code="200">Returns the specified List&lt;CompanyProfileDto&gt;.</response>
        [HttpGet("api/company-profiles")]
        [ProducesResponseType(typeof(List<CompanyProfileDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<CompanyProfileDto>>> GetCompanyProfiles(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetCompanyProfilesQuery(), cancellationToken);
            return Ok(result);
        }
    }
}