using ProjectManager.Application.Ports;

namespace ProjectManager.Application.UseCases.Projects.Get
{
    public sealed class GetProjectByIdUseCase(IProjectRepository projectRepository) : IGetProjectByIdUseCase
    {
        private readonly IProjectRepository _projectRepository = projectRepository;

        public async Task<GetProjectByIdResponse> Execute(Guid projectId, CancellationToken ct = default)
        {
            var project = await _projectRepository.GetByIdAsync(projectId, ct)
                ?? throw new InvalidOperationException("Project no found");

            return new GetProjectByIdResponse
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Status = project.Status,
                OrganizationId = project.OrganizationId,
                OwnerId = project.OwnerId,
                CreatedAt = project.CreatedAt,
                UpdatedAt = project.UpdatedAt
            };
        }
    }
}
