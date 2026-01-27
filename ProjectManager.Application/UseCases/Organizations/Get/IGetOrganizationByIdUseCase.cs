namespace ProjectManager.Application.UseCases.Organizations.Get
{
    public interface IGetOrganizationByIdUseCase
    {
        Task<GetOrganizationByIdResponse> Execute(Guid organizationId, CancellationToken ct = default);
    }
}
