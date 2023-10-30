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

    public async Task SendConfirmationAsync(User user, string token)
    {
        MailAddress from = new(_smtpConfiguration.SenderAddress, _smtpConfiguration.SenderName, Encoding.UTF8);
        MailAddress to = new(user.Email!);

        MailMessage message = new(from, to);
        message.Subject = "Confirm Email";
        message.SubjectEncoding = Encoding.UTF8;
        message.Body = $"""
                        <a href="{_configuration["WebApiUrl"]}/email-confirmation?token={token}">Confirm</a> your email.
                        """;
        message.IsBodyHtml = true;
        message.BodyEncoding = Encoding.UTF8;

        await _smtpClient.SendMailAsync(message);
    }
}