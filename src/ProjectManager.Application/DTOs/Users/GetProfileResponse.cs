namespace ProjectManager.Application.DTOs.Users
{
    public sealed class GetProfileResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
