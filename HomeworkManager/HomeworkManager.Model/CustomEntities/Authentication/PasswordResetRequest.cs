namespace HomeworkManager.Model.CustomEntities.Authentication;

public class PasswordResetRequest
{
    public required string Password { get; set; }
    public required string Token { get; set; }
}