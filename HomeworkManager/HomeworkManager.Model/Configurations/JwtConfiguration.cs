namespace HomeworkManager.Model.Configurations;

public class JwtConfiguration
{
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public required string Subject { get; set; }
    public required string Key { get; set; }
    public required int ExpirationMinutes { get; set; }
}