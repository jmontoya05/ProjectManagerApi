using ProjectManager.Application.Exceptions;

namespace ProjectManager.Api.Middlewares
{
    public class TenantContextMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var orgClaim = context.User.Claims.FirstOrDefault(c => c.Type == "OrganizationId");
                if (orgClaim == null || string.IsNullOrWhiteSpace(orgClaim.Value))
                    throw new UnauthorizedException(
                        "Organization context is required. Obtain an org-scoped token via POST /auth/organization.");
            }

            await next(context);
        }
    }
}