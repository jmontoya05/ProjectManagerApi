using ProjectManager.Application.DTOs.Teams;

namespace ProjectManager.Application.UseCases.Teams.List
{
    public interface IListTeamsUseCase
    {
        Task<IEnumerable<ListTeamsResponse>> Execute(CancellationToken ct = default);
    }
}
