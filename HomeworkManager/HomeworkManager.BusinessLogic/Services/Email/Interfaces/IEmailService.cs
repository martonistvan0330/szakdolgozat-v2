using HomeworkManager.Model.Entities;

namespace HomeworkManager.BusinessLogic.Services.Email.Interfaces;

public interface IEmailService
{
    Task SendConfirmationEmailAsync(User user, string token);
    Task SendPasswordRecoveryEmailAsync(User user, string token);
}