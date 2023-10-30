namespace HomeworkManager.Model.Entities;

public class EmailConfirmationToken
{
    public int EmailConfirmationTokenId { get; set; }
    public required string Token { get; set; }
    public bool IsActive { get; set; } = true;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}