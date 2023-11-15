using FluentResults;
using HomeworkManager.Model.CustomEntities.User;
using HomeworkManager.Model.Entities;

namespace HomeworkManager.BusinessLogic.Services.Email.Interfaces;

public interface IEmailService
{
    Task<Result> SendConfirmationEmailAsync(UserModel userModel, string token, CancellationToken cancellationToken = default);
    Task<Result> SendPasswordRecoveryEmailAsync(UserModel userModel, string token, CancellationToken cancellationToken = default);
}