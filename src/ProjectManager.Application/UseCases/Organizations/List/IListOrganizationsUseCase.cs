using ProjectManager.Application.DTOs.Organizations;

namespace ProjectManager.Application.UseCases.Organizations.List
{
    public interface IListOrganizationsUseCase
    {
        Task<IEnumerable<ListOrganizationsResponse>> Execute(CancellationToken ct = default);
    }
}
