namespace ProjectManager.Application.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(Guid userId, string email);
        string GenerateRefreshToken();
    }
}
