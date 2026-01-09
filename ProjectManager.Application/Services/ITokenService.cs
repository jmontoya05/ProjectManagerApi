namespace ProjectManager.Application.Services
{
    public interface ITokenService
    {
        string GenerateAccesToken(Guid userId, string email);
        string GenerateRefreshToken();
    }
}
