using ProjectManager.Application.DTOs.Requests;
using ProjectManager.Application.DTOs.Responses;

namespace ProjectManager.Application.UseCases.Auth.SelectOrganization
{
    public interface ISelectOrganizationUseCase
    {
        Task<SelectOrganizationResponse> Execute(SelectOrganizationRequest request, CancellationToken ct = default);
    }
}
