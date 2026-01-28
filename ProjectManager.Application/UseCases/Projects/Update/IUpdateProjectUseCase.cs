namespace ProjectManager.Application.UseCases.Projects.Update
{
    public interface IUpdateProjectUseCase
    {
        Task Execute(UpdateProjectRequest request, Guid projectId, Guid organizationId, Guid currentUserId, CancellationToken ct = default);
    }
}
