namespace BlogAPI.Models;

public class VerificationToken
{
    public long Id { get; set; }
    public string Email { get; set; } = null!;
    public string Token { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
}