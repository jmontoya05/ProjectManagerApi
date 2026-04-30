namespace ProjectManager.Domain.Entities
{
    public class RefreshToken : EntityBase
    {
        public Guid UserId { get; init; }
        public string Token { get; init; } = null!;
        public DateTime ExpiresAt { get; init; }
        public DateTime? RevokedAt { get; set; }

        //Navigation properties
        public virtual User User { get; init; } = null!;
    }
}
