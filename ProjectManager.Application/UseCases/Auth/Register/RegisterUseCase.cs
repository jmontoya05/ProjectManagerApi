using ProjectManager.Application.Ports;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.UseCases.Auth.Register
{
    public sealed class RegisterUseCase(IUserRepository userRepository) : IRegisterUseCase
    {
        private readonly IUserRepository _userRepository = userRepository;
        public async Task<Guid> Execute(RegisterRequest registerRequest, CancellationToken ct = default)
        {
            var exists = await _userRepository.ExistsByEmailAsync(registerRequest.Email, ct);

            if (exists)
                throw new InvalidOperationException("Email already registered.");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = registerRequest.Name,
                Email = registerRequest.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password),
                DisplayName = registerRequest.Name,
                Status = "Active",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user, ct);

            return user.Id;
        }
    }
}
