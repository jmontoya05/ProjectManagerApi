using ProjectManager.Application.DTOs.Responses;
using ProjectManager.Application.Ports;

namespace ProjectManager.Application.UseCases.Teams.List
{
    public sealed class ListTeamsUseCase(ITeamRepository teamRepository) : IListTeamsUseCase
    {
        private readonly ITeamRepository _teamRepository = teamRepository;

        public async Task<IEnumerable<TeamResponse>> Execute(Guid organizationId, CancellationToken ct = default)
        {
            var teams = await _teamRepository.GetByOrganizationAsync(organizationId, ct);

            return teams.Select(t => new TeamResponse
            {
                Id = t.Id,
                Name = t.Name,
                OrganizationId = t.OrganizationId,
                CreatedAt = t.CreatedAt
            });
        }
    }
}
