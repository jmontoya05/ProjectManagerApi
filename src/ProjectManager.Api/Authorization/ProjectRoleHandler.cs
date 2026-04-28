using Microsoft.AspNetCore.Authorization;
using ProjectManager.Application.Services;
using ProjectManager.Application.Ports;
using Microsoft.AspNetCore.Http;
using System.Linq;

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
            var userId = _tenantContext.UserId;
            var orgId = _tenantContext.OrganizationId;
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

            var roles = await _userRepository.GetProjectRolesAsync(Guid.Parse(userId), projectId);
            if (roles.Contains(requirement.RequiredRole))
                context.Succeed(requirement);
            else
                context.Fail();
        }
    }
}
