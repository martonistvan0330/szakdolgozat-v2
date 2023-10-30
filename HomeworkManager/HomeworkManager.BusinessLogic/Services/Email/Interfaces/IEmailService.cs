using HomeworkManager.Model.Entities;

namespace HomeworkManager.BusinessLogic.Services.Email.Interfaces;

public interface IEmailService
{
    Task SendConfirmationAsync(User user, string token);
}