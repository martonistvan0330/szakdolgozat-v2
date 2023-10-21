namespace HomeworkManager.Model.CustomEntities.Authentication;

public class AuthenticationResponse
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
    public required DateTime Expiration { get; set; }
}