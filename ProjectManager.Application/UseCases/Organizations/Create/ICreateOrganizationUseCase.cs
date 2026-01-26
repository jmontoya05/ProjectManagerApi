namespace ProjectManager.Application.UseCases.Organizations.Create
{
    public interface ICreateOrganizationUseCase
    {
        Task<Guid> Execute(CreateOrganizationRequest request, Guid userId, CancellationToken ct = default);
    }
}
