namespace ProjectManager.Application.UseCases.Projects.Create
{
    public interface ICreateProjectUseCase
    {
        Task<Guid> Execute(CreateProjectRequest request, Guid organizationId, Guid userId, CancellationToken ct = default);
    }
}
