namespace ProjectManager.Application.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(Guid userId, string email, Guid? organizationId = null, IEnumerable<string>? roles = null);
        string GenerateRefreshToken();
    }
}
