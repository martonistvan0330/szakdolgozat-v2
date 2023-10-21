namespace HomeworkManager.Model.Entities;

public class AccessToken
{
    public int AccessTokenId { get; set; }
    public required string Token { get; set; }
    public bool IsActive { get; set; } = true;
        
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public RefreshToken RefreshToken { get; set; } = null!;
}