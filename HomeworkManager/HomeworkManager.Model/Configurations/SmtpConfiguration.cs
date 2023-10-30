namespace HomeworkManager.Model.Configurations;

public class SmtpConfiguration
{
    public required string Server { get; set; }
    public required int Port { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string SenderAddress { get; set; }
    public required string SenderName { get; set; }
}