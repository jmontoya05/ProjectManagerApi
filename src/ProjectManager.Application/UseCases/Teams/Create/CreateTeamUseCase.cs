using ProjectManager.Application.DTOs.Teams;
using ProjectManager.Application.Exceptions;
using ProjectManager.Application.Ports;
using ProjectManager.Application.Services;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.UseCases.Teams.Create
{
    public sealed class CreateTeamUseCase(
        ITeamRepository teamRepository, 
        ITenantContext tenantContext
    ) : ICreateTeamUseCase
    {
        private readonly ITeamRepository _teamRepository = teamRepository;
        private readonly ITenantContext _tenantContext = tenantContext;

        public async Task<Guid> Execute(CreateTeamRequest request, CancellationToken ct = default)
        {
            var orgId = Guid.TryParse(_tenantContext.OrganizationId, out var id) ? id
                : throw new UnauthorizedException("Invalid organization context.");

            var team = new Team
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                OrganizationId = orgId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _teamRepository.AddAsync(team, ct);

            return team.Id;
        }
    }
}
