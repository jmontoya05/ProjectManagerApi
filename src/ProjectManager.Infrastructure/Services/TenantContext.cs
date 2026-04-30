using Microsoft.AspNetCore.Http;
using ProjectManager.Application.Services;
using System.Security.Claims;

namespace ProjectManager.Infrastructure.Services
{
    public class TenantContext(
        IHttpContextAccessor httpContextAccessor
    ) : ITenantContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
  
        public string? OrganizationId =>
            _httpContextAccessor.HttpContext?.User.FindFirst("OrganizationId")?.Value;

        public string? UserId =>
            _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public Guid GetOrganizationIdOrThrow() => 
            GetRequiredGuidClaim("OrganizationId");

        public Guid GetUserIdOrThrow() => 
            GetRequiredGuidClaim(ClaimTypes.NameIdentifier);

        public Guid? TryGetOrganizationId() => 
            TryGetGuidClaim("OrganizationId");

        public Guid? TryGetUserId() => 
            TryGetGuidClaim(ClaimTypes.NameIdentifier);
        
        private Guid GetRequiredGuidClaim(string claimType)
        {
            var value = GetClaimValue(claimType);

            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidOperationException(
                    $"Missing required claim '{claimType}' in the current context.");

            if (!Guid.TryParse(value, out var guid))
                throw new InvalidOperationException(
                    $"Invalid GUID format for claim '{claimType}'.");

            return guid;
        }
        
        private string? GetClaimValue(string claimType)
        {
            return _httpContextAccessor
                .HttpContext?
                .User?
                .FindFirst(claimType)?
                .Value;
        }
        
        private Guid? TryGetGuidClaim(string claimType)
        {
            var value = GetClaimValue(claimType);

            if (string.IsNullOrWhiteSpace(value))
                return null;

            return Guid.TryParse(value, out var guid) ? guid : null;
        }
    }
}
