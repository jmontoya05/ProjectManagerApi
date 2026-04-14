using Microsoft.AspNetCore.Http;
using ProjectManager.Application.Services;
using System.Security.Claims;

namespace ProjectManager.Infrastructure.Services
{
    /// <summary>
    /// Provides tenant and user context from the current HTTP request.
    /// </summary>
    public class TenantContext : ITenantContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? OrganizationId =>
            _httpContextAccessor.HttpContext?.User.FindFirst("OrganizationId")?.Value;

        public string? UserId =>
            _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}
