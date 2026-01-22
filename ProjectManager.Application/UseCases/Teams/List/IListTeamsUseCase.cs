using ProjectManager.Application.DTOs.Responses;

namespace ProjectManager.Application.UseCases.Teams.List
{
    public interface IListTeamsUseCase
    {
        Task<IEnumerable<TeamResponse>> Execute(Guid organizationId, CancellationToken ct = default);
    }
}
