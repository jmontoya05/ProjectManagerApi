using ProjectManager.Application.DTOs;
using ProjectManager.Application.DTOs.Requests;
using ProjectManager.Application.DTOs.Responses;
using ProjectManager.Application.Ports;
using ProjectManager.Application.Services;

namespace ProjectManager.Application.UseCases.Auth.Login
{
    public class LoginUseCase(IUserRepository userRepository, ITokenService tokenService) : ILoginUseCase
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<LoginResponse> Execute(LoginRequest request, CancellationToken ct = default)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email, ct)
                ?? throw new InvalidOperationException("The email doesn't exists");

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new InvalidOperationException("Invalid password");

            if (user.Status != "Active")
                throw new InvalidOperationException("User is blocked");

            var accesToken = _tokenService.GenerateAccessToken(user.Id, user.Email);
            var refreshToken = _tokenService.GenerateRefreshToken();

            await _userRepository.SaveRefreshTokenAsync(user.Id, refreshToken, DateTime.UtcNow.AddDays(7), ct);

            return new LoginResponse
            {
                AccessToken = accesToken,
                ExpiresInSeconds = 15 * 60,
                RefreshToken = refreshToken,
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    DisplayName = user.DisplayName,
                    Status = user.Status
                }
            };
        }
    }
}
