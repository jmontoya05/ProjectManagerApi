namespace ProjectManager.Application.Services
{
    /// <summary>
    /// Provides access to the current tenant (organization) and user context from the request.
    /// </summary>
    public interface ITenantContext
    {
        /// <summary>
        /// Gets the current organization ID from the request context (JWT claims).
        /// </summary>
        string? OrganizationId { get; }

        /// <summary>
        /// Gets the current user ID from the request context (JWT claims).
        /// </summary>
        string? UserId { get; }
    }
}
