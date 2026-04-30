using Microsoft.AspNetCore.Authorization;
using ProjectManager.Application.Services;
using ProjectManager.Application.Ports;

namespace ProjectManager.Api.Authorization
{
    public class ProjectRoleHandler(
        ITenantContext tenantContext, 
        IUserRepository userRepository, 
        IHttpContextAccessor httpContextAccessor
    ) : AuthorizationHandler<ProjectRoleRequirement>
    {
        private readonly ITenantContext _tenantContext = tenantContext;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ProjectRoleRequirement requirement)
        {
            var userId = _tenantContext.TryGetUserId();
            var orgId = _tenantContext.TryGetOrganizationId();
            if (userId == null || orgId == null)
            {
                context.Fail();
                return;
            }
            
            var routeValues = _httpContextAccessor.HttpContext?.Request.RouteValues;
            if (routeValues == null || 
                !routeValues.TryGetValue("projectId", out var projectIdValue) || 
                !Guid.TryParse(projectIdValue?.ToString(), out var projectId))
            {
                context.Fail();
                return;
            }

            var roles = await _userRepository.GetProjectRolesAsync(userId.Value, projectId);
            var enumerable = roles.ToList();
            if (enumerable.Contains(requirement.RequiredRole) || (requirement.AllowEscalation && enumerable.Contains("ProjectManager")))
                context.Succeed(requirement);
            else
                context.Fail();
        }
    }
}
