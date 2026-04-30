using Microsoft.AspNetCore.Authorization;

namespace ProjectManager.Api.Authorization
{
    public class ProjectRoleRequirement(string requiredRole, bool allowEscalation = false) : IAuthorizationRequirement
    {
        public string RequiredRole { get; } = requiredRole;
        public bool AllowEscalation { get; } = allowEscalation;
    }
}
