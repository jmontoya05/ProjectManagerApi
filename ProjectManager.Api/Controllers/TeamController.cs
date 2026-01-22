using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Api.Middlewares;
using ProjectManager.Application.DTOs.Requests;
using ProjectManager.Application.UseCases.Teams.Create;
using ProjectManager.Application.UseCases.Teams.Get;
using ProjectManager.Application.UseCases.Teams.List;

namespace ProjectManager.Api.Controllers
{
    [ApiController]
    [Route("org/{orgId}/teams")]
    [Authorize]
    public class TeamController(ICreateTeamUseCase createTeamUseCase, IListTeamsUseCase listTeamsUseCase, IGetTeamByIdUseCase getTeamByIdUseCase) : ControllerBase
    {
        private readonly ICreateTeamUseCase _createTeamUseCase = createTeamUseCase;
        private readonly IListTeamsUseCase _listTeamsUseCase = listTeamsUseCase;
        private readonly IGetTeamByIdUseCase _getTeamByIdUseCase = getTeamByIdUseCase;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTeamRequest request, [FromRoute] Guid orgId)
        {
            var teamId = await _createTeamUseCase.Execute(request, orgId);

            return CreatedAtAction(nameof(Create), new { orgId, id = teamId }, new { id = teamId });
        }

        [HttpGet]
        public async Task<IActionResult> List([FromRoute] Guid orgId)
        {
            try
            {
                var teams = await _listTeamsUseCase.Execute(orgId);
                return Ok(teams);
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    new
                    {
                        correlationId = GetCorrelationId(),
                        errorCode = "INTERNAL_ERROR",
                        message = "An unexpected error occurred"
                    });
            }
        }

        [HttpGet("{teamId}")]
        public async Task<IActionResult> GetById([FromRoute] Guid teamId)
        {
            try
            {
                var team = await _getTeamByIdUseCase.Execute(teamId);
                return Ok(team);
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("Team not found"))
            {

                return NotFound(
                    new
                    {
                        correlationId = GetCorrelationId(),
                        errorCode = "TEAM_NOT_FOUND",
                        message = ex.Message
                    });
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    new
                    {
                        correlationId = GetCorrelationId(),
                        errorCode = "INTERNAL_ERROR",
                        message = "An unexpected error occurred"
                    });
            }
        }

        private string GetCorrelationId() =>
            ExceptionHandlingMiddleware.GetCorrelationId(HttpContext);
    }
}
