namespace ProjectManager.Application.UseCases.Organizations.List
{
    public interface IListOrganizationsUseCase
    {
        Task<IEnumerable<ListOrganizationsResponse>> Execute(Guid userId, CancellationToken ct = default);
    }
}
