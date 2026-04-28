using ProjectManager.Application.Services;

namespace ProjectManager.Infrastructure.Services;

public class MockEmailService : IEmailService
{
    public Task SendInvitationEmailAsync(string email, string invitationToken)
    {
        // Simulate sending email
        System.Diagnostics.Debug.WriteLine($"Mock email sent to {email} with token: {invitationToken}");
        return Task.CompletedTask;
    }
}