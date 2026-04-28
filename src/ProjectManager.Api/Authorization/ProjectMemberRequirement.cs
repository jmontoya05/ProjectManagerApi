using Microsoft.AspNetCore.Authorization;

namespace ProjectManager.Api.Authorization
{
    public class ProjectMemberRequirement(string requiredRole = "ProjectMember") : IAuthorizationRequirement
    {
        public string RequiredRole { get; } = requiredRole;
    }
}
