using ProjectManager.Application.DTOs.Auth;

namespace ProjectManager.Application.UseCases.Auth.Invite;

public interface ICompleteInvitationUseCase
{
    Task<Guid> Execute(CompleteInvitationRequest request, CancellationToken ct = default);
}