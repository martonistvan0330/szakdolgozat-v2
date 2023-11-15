using FluentValidation;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.Model.CustomEntities.User;

namespace HomeworkManager.API.Validation.Authentication;

public class NewUserValidator : AbstractValidator<NewUser>
{
    public NewUserValidator
    (
        IUserManager userManager,
        EmailValidator emailValidator,
        PasswordValidator passwordValidator
    )
    {
        RuleFor(x => x.Username)
            .MustAsync(async (username, cancellationToken) =>
                await userManager.UsernameAvailableAsync(username, cancellationToken));

        RuleFor(x => x.Password)
            .SetValidator(passwordValidator);

        RuleFor(x => x.Email)
            .SetValidator(emailValidator)
            .MustAsync(async (email, cancellationToken) =>
                await userManager.EmailAvailableAsync(email, cancellationToken));
    }
}