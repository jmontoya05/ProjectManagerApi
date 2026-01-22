using ProjectManager.Application.DTOs.Requests;
using ProjectManager.Application.Ports;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.UseCases.Teams.Create
{
    public sealed class CreateTeamUseCase(ITeamRepository teamRepository) : ICreateTeamUseCase
    {
        private readonly ITeamRepository _teamRepository = teamRepository;

        public async Task<Guid> Execute(CreateTeamRequest request, Guid organizationId, CancellationToken ct = default)
        {
            var team = new Team
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                OrganizationId = organizationId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _teamRepository.AddAsync(team, ct);

            return team.Id;
        }
    }
}
