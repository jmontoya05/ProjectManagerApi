using ProjectManager.Application.DTOs.Teams;

namespace ProjectManager.Application.UseCases.Teams.Get
{
    public interface IGetTeamByIdUseCase
    {
        Task<GetTeamByIdResponse> Execute(Guid teamId, CancellationToken ct = default);
    }
}
