using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.DTOs.Teams;
using ProjectManager.Application.UseCases.Teams.AddTeamMember;
using ProjectManager.Application.UseCases.Teams.Create;
using ProjectManager.Application.UseCases.Teams.Get;
using ProjectManager.Application.UseCases.Teams.List;

namespace ProjectManager.Api.Controllers
{
    [ApiController]
    [Route("org/{orgId:guid}/teams")]
    [Authorize]
    public sealed class TeamController(
        ICreateTeamUseCase createTeamUseCase, 
        IListTeamsUseCase listTeamsUseCase, 
        IGetTeamByIdUseCase getTeamByIdUseCase, 
        IAddTeamMemberUseCase addTeamMemberUseCase
        ) : ControllerBase
    {
        private readonly ICreateTeamUseCase _createTeamUseCase = createTeamUseCase;
        private readonly IListTeamsUseCase _listTeamsUseCase = listTeamsUseCase;
        private readonly IGetTeamByIdUseCase _getTeamByIdUseCase = getTeamByIdUseCase;
        private readonly IAddTeamMemberUseCase _addTeamMemberUseCase = addTeamMemberUseCase;

        [HttpPost]
        [Authorize(Policy = "OrgAdmin")]
        public async Task<IActionResult> Create([FromRoute] Guid orgId, [FromBody] CreateTeamRequest request, CancellationToken ct)
        {
            var teamId = await _createTeamUseCase.Execute(request, ct);
            return CreatedAtAction(nameof(Create), new { orgId, id = teamId }, new { id = teamId });
        }

        [HttpGet]
        [Authorize(Policy = "OrgMember")]
        public async Task<IActionResult> List([FromRoute] Guid orgId, CancellationToken ct)
        {
            var teams = await _listTeamsUseCase.Execute(ct);
            return Ok(teams);
        }

        [HttpGet("{teamId:guid}")]
        [Authorize(Policy = "OrgMember")]
        public async Task<IActionResult> GetById([FromRoute] Guid orgId, [FromRoute] Guid teamId, CancellationToken ct)
        {
            var team = await _getTeamByIdUseCase.Execute(teamId, ct);
            return Ok(team);
        }

        [HttpPost("{teamId:guid}/members")]
        [Authorize(Policy = "OrgAdmin")]
        public async Task<IActionResult> AddMember([FromRoute] Guid orgId, [FromRoute] Guid teamId, [FromBody] AddTeamMemberRequest request, CancellationToken ct)
        {
            await _addTeamMemberUseCase.Execute(request, teamId, ct);
            return NoContent();
        }
    }
}
