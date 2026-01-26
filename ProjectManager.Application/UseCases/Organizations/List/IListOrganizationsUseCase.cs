using ProjectManager.Application.DTOs.Responses;

namespace ProjectManager.Application.UseCases.Organizations.List
{
    public interface IListOrganizationsUseCase
    {
        Task<IEnumerable<OrganizationResponse>> Execute(Guid userId, CancellationToken ct = default);
    }
}
