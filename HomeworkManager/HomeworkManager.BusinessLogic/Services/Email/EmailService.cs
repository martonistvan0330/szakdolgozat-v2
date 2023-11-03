using System.Net;
using System.Net.Mail;
using System.Text;
using HomeworkManager.BusinessLogic.Services.Email.Interfaces;
using HomeworkManager.Model.Configurations;
using HomeworkManager.Model.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace HomeworkManager.BusinessLogic.Services.Email;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly SmtpClient _smtpClient;
    private readonly SmtpConfiguration _smtpConfiguration;

    public EmailService(IOptions<SmtpConfiguration> smtpConfiguration, IConfiguration configuration)
    {
        _configuration = configuration;
        _smtpConfiguration = smtpConfiguration.Value;
        _smtpClient = new SmtpClient
        {
            Host = _smtpConfiguration.Server,
            Port = _smtpConfiguration.Port,
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_smtpConfiguration.Username, _smtpConfiguration.Password)
        };
    }

    public async Task SendConfirmationEmailAsync(User user, string token)
    {
        var subject = "Confirm Email";
        var body = $"""
                    <a href="{_configuration["WebApiUrl"]}/email-confirmation?token={token}">Confirm</a> your email.
                    """;
        await SendEmailAsync(user, subject, body);
    }

    public async Task SendPasswordRecoveryEmailAsync(User user, string token)
    {
        var subject = "Password Recovery";
        var body = $"""
                    <a href="{_configuration["WebApiUrl"]}/recover-password?token={token}">Recover your password.</a>
                    """;
        await SendEmailAsync(user, subject, body);
    }

    private async Task SendEmailAsync(User user, string subject, string body)
    {
        MailAddress from = new(_smtpConfiguration.SenderAddress, _smtpConfiguration.SenderName, Encoding.UTF8);
        MailAddress to = new(user.Email!);

        MailMessage message = new(from, to);
        message.Subject = subject;
        message.SubjectEncoding = Encoding.UTF8;
        message.Body = body;
        message.IsBodyHtml = true;
        message.BodyEncoding = Encoding.UTF8;

        await _smtpClient.SendMailAsync(message);
    }
}