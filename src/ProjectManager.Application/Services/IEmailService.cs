using System.Threading.Tasks;

namespace ProjectManager.Application.Services
{
    public interface IEmailService
    {
        Task SendInvitationEmailAsync(string email, string invitationToken);
    }
}
