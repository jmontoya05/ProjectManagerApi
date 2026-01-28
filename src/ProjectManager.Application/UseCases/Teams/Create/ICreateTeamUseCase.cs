namespace ProjectManager.Application.UseCases.Teams.Create
{
    public interface ICreateTeamUseCase
    {
        Task<Guid> Execute(CreateTeamRequest request, Guid organizationId, CancellationToken ct = default);
    }
}
