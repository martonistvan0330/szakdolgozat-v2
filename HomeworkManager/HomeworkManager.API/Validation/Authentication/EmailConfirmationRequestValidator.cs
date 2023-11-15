using FluentValidation;
using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.CustomEntities.Authentication;

namespace HomeworkManager.API.Validation.Authentication;

public class EmailConfirmationRequestValidator : AbstractValidator<EmailConfirmationRequest>
{
    public EmailConfirmationRequestValidator(IEmailConfirmationTokenRepository emailConfirmationTokenRepository)
    {
        RuleFor(x => x.Token)
            .MustAsync(async (emailConfirmationToken, cancellationToken) =>
                await emailConfirmationTokenRepository.IsActiveAsync(emailConfirmationToken, cancellationToken));
    }
}