namespace ProjectManager.Application.UseCases.Projects.List
{
    public interface IListProjectsUseCase
    {
        Task<IEnumerable<ListProjectsResponse>> Execute(Guid organizationId, CancellationToken ct = default);
    }
}
