namespace ProjectManager.Application.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(Guid userId, string email, Guid organizationId, IEnumerable<string> roles);
        string GenerateRefreshToken();
    }
}
