namespace ProjectManager.Application.Services
{
    public interface ITenantContext
    {
        Guid GetOrganizationIdOrThrow();
        Guid GetUserIdOrThrow();
        Guid? TryGetOrganizationId();
        Guid? TryGetUserId();
    }
}
