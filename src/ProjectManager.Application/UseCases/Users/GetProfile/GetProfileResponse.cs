namespace ProjectManager.Application.UseCases.Users.GetProfile
{
    public sealed class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
