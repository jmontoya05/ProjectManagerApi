namespace ProjectManager.Application.DTOs.Auth
{
    public sealed class LoginResponse
    {
        public UserDto User { get; set; } = null!;
        public IEnumerable<OrganizationDto> Organizations { get; set; } = [];
        public string RefreshToken { get; set; } = null!;
    }
}
