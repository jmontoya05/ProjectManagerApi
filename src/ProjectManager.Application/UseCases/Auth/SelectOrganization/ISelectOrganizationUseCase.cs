namespace ProjectManager.Application.UseCases.Auth.SelectOrganization
{
    public interface ISelectOrganizationUseCase
    {
        Task<SelectOrganizationResponse> Execute(SelectOrganizationRequest request, CancellationToken ct = default);
    }
}
