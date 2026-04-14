using ProjectManager.Application.DTOs.Projects;
using ProjectManager.Application.Ports;
using ProjectManager.Application.Exceptions;

namespace ProjectManager.Application.UseCases.Projects.Get
{
    public sealed class GetProjectByIdUseCase(IProjectRepository projectRepository) : IGetProjectByIdUseCase
    {
        private readonly IProjectRepository _projectRepository = projectRepository;

        public async Task<GetProjectByIdResponse> Execute(Guid projectId, Guid organizationId, CancellationToken ct = default)
        {
            var project = await _projectRepository.GetByIdAsync(projectId, ct)
                ?? throw new NotFoundException("Project not found", "Project", projectId);

            if (project.OrganizationId != organizationId)
                throw new ForbiddenException("You do not have access to this project");
    
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
