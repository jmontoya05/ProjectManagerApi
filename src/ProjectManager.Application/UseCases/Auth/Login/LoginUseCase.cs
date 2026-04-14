using ProjectManager.Application.DTOs.Auth;
using ProjectManager.Application.Ports;
using ProjectManager.Application.Services;
using ProjectManager.Application.Exceptions;

namespace ProjectManager.Application.UseCases.Auth.Login
{
    public class LoginUseCase(IUserRepository userRepository, ITokenService tokenService) : ILoginUseCase
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<LoginResponse> Execute(LoginRequest request, CancellationToken ct = default)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email, ct)
                ?? throw new NotFoundException($"User with email '{request.Email}' not found", "User", request.Email);

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new BusinessRuleException("Invalid password", "INVALID_PASSWORD");

            if (user.Status != "Active")
                throw new BusinessRuleException("User is blocked", "USER_BLOCKED");

            var organizations = await _userRepository.GetUserOrganizationsAsync(user.Id, ct);
            var organizationsDto = organizations
                .Select(o => new OrganizationDto { Id = o.Id, Name = o.Name })
                .ToList();

            var refreshToken = _tokenService.GenerateRefreshToken();
            await _userRepository.SaveRefreshTokenAsync(user.Id, refreshToken, DateTime.UtcNow.AddDays(7), ct);

            return new LoginResponse
            {
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    DisplayName = user.DisplayName,
                    Status = user.Status
                },
                Organizations = organizationsDto,
                RefreshToken = refreshToken
            };
        }
    }
}
