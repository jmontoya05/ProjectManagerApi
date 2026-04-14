using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Api.Middlewares;
using ProjectManager.Application.DTOs.Teams;
using ProjectManager.Application.Exceptions;
using ProjectManager.Application.UseCases.Teams.AddTeamMember;
using ProjectManager.Application.UseCases.Teams.Create;
using ProjectManager.Application.UseCases.Teams.Get;
using ProjectManager.Application.UseCases.Teams.List;
using System.Security.Claims;

namespace ProjectManager.Api.Controllers
{
    [ApiController]
    [Route("org/{orgId}/teams")]
    [Authorize]
    public sealed class TeamController(ICreateTeamUseCase createTeamUseCase, IListTeamsUseCase listTeamsUseCase, IGetTeamByIdUseCase getTeamByIdUseCase, IAddTeamMemberUseCase addTeamMemberUseCase) : ControllerBase
    {
        private readonly ICreateTeamUseCase _createTeamUseCase = createTeamUseCase;
        private readonly IListTeamsUseCase _listTeamsUseCase = listTeamsUseCase;
        private readonly IGetTeamByIdUseCase _getTeamByIdUseCase = getTeamByIdUseCase;
        private readonly IAddTeamMemberUseCase _addTeamMemberUseCase = addTeamMemberUseCase;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTeamRequest request, [FromRoute] Guid orgId)
        {
            var teamId = await _createTeamUseCase.Execute(request, orgId);

            return CreatedAtAction(nameof(Create), new { orgId, id = teamId }, new { id = teamId });
        }

        [HttpGet]
        public async Task<IActionResult> List([FromRoute] Guid orgId)
        {
            var teams = await _listTeamsUseCase.Execute();
            return Ok(teams);
        }

        [HttpGet("{teamId}")]
        public async Task<IActionResult> GetById([FromRoute] Guid teamId)
        {
            var team = await _getTeamByIdUseCase.Execute(teamId);
            return Ok(team);
        }

        [HttpPost("{teamId}/members")]
        public async Task<IActionResult> AddMember([FromBody] AddTeamMemberRequest request, [FromRoute] Guid teamId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(
                    new
                    {
                        correlationId = GetCorrelationId(),
                        errorCode = "INVALID_TOKEN",
                        message = "Invalid token."
                    });
            }

            await _addTeamMemberUseCase.Execute(request, teamId, userId);

            return NoContent();
        }

        private string GetCorrelationId() =>
            ExceptionHandlingMiddleware.GetCorrelationId(HttpContext);
    }
}
