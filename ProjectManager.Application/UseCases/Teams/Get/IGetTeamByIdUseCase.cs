using ProjectManager.Application.DTOs.Responses;

namespace ProjectManager.Application.UseCases.Teams.Get
{
    public interface IGetTeamByIdUseCase
    {
        Task<TeamResponse> Execute(Guid teamId, CancellationToken ct = default);
    }
}
