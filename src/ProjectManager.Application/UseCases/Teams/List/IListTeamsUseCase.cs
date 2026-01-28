namespace ProjectManager.Application.UseCases.Teams.List
{
    public interface IListTeamsUseCase
    {
        Task<IEnumerable<ListTeamsResponse>> Execute(Guid organizationId, CancellationToken ct = default);
    }
}
