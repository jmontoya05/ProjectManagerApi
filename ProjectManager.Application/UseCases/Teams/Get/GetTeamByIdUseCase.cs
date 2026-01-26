using ProjectManager.Application.Ports;

namespace ProjectManager.Application.UseCases.Teams.Get
{
    public sealed class GetTeamByIdUseCase(ITeamRepository teamRepository) : IGetTeamByIdUseCase
    {
        private readonly ITeamRepository _teamRepository = teamRepository;

        public async Task<GetTeamByIdResponse> Execute(Guid teamId, CancellationToken ct = default)
        {
            var team = await _teamRepository.GetByIdAsync(teamId, ct)
                ?? throw new InvalidOperationException("Team not found");

            return new GetTeamByIdResponse
            {
                Id = team.Id,
                Name = team.Name,
                OrganizationId = team.OrganizationId,
                CreatedAt = team.CreatedAt
            };
        }
    }
}
