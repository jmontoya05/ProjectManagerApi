using Microsoft.AspNetCore.Authorization;
using ProjectManager.Application.Ports;
using ProjectManager.Application.Services;

namespace ProjectManager.Api.Authorization
{
    public class ProjectMemberHandler(
        ITenantContext tenantContext, 
        IUserRepository userRepository, 
        IHttpContextAccessor httpContextAccessor
    ) : AuthorizationHandler<ProjectMemberRequirement>
    {
        private readonly ITenantContext _tenantContext = tenantContext;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ProjectMemberRequirement requirement)
        {
            var userId = _tenantContext.UserId;
            var orgId = _tenantContext.OrganizationId;
            
            var routeValues = _httpContextAccessor.HttpContext?.Request.RouteValues;
            if (userId == null || orgId == null || 
                routeValues == null || 
                !routeValues.TryGetValue("projectId", out var projectIdValue) || 
                !Guid.TryParse(projectIdValue?.ToString(), out var projectId))
            {
                context.Fail();
                return;
            }

            var roles = await _userRepository.GetProjectRolesAsync(Guid.Parse(userId), projectId);
            var enumerable = roles as string[] ?? roles.ToArray();
            if (enumerable.Contains(requirement.RequiredRole) || enumerable.Contains("ProjectManager"))
                context.Succeed(requirement);
            else
                context.Fail();
        }
    }
}
