namespace ProjectManager.Api.Middlewares
{
    public class OrganizationMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task Invoke(HttpContext context)
        {
            if (context.User.Identity == null || !context.User.Identity.IsAuthenticated)
            {
                await _next(context);
                return;
            }

            var tokenOrg = context.User.FindFirst("OrganizationId")?.Value;
            var routeOrg = context.Request.RouteValues["orgId"]?.ToString();

            if (!string.IsNullOrEmpty(routeOrg) && !string.Equals(tokenOrg, routeOrg, StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Organization mismatch");
                return;
            }

            await _next(context);
        }
    }
}
