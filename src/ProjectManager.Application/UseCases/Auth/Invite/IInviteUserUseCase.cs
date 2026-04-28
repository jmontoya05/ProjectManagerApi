using ProjectManager.Application.DTOs.Auth;

namespace ProjectManager.Application.UseCases.Auth.Invite
{
    public interface IInviteUserUseCase
    {
        Task<InviteUserResponse> Execute(InviteUserRequest request, Guid adminUserId, CancellationToken ct = default);
    }
}
