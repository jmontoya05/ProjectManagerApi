using ProjectManager.Application.Ports;

namespace ProjectManager.Application.UseCases.Projects.Get
{
    public sealed class GetProjectByIdUseCase(IProjectRepository projectRepository) : IGetProjectByIdUseCase
    {
        private readonly IProjectRepository _projectRepository = projectRepository;

        public async Task<GetProjectByIdResponse> Execute(Guid projectId, Guid organizationId, CancellationToken ct = default)
        {
            var project = await _projectRepository.GetByIdAsync(projectId, ct)
                ?? throw new InvalidOperationException("Project no found");

            if (project.OrganizationId != organizationId)
                throw new InvalidOperationException("You do not have access to this project");
    
            return new GetProjectByIdResponse
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Status = project.Status,
                OrganizationId = project.OrganizationId,
                OwnerId = project.OwnerId,
                CreatedAt = project.CreatedAt,
                CreatedBy = project.CreatedBy,
                UpdatedAt = project.UpdatedAt,
                UpdatedBy = project.UpdatedBy
            };
        }
    }
}
