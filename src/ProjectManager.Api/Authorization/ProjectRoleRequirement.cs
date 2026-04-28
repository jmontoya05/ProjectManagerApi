using Microsoft.AspNetCore.Authorization;

namespace ProjectManager.Api.Authorization
{
    public class ProjectRoleRequirement(string requiredRole) : IAuthorizationRequirement
    {
        public string RequiredRole { get; } = requiredRole;
    }
}
