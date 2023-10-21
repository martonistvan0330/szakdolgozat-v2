namespace HomeworkManager.Model.CustomEntities.Authentication;

public class RevokeRequest
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}