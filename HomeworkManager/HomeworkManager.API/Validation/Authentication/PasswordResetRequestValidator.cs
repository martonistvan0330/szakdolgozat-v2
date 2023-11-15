using FluentValidation;
using HomeworkManager.Model.CustomEntities.Authentication;

namespace HomeworkManager.API.Validation.Authentication;

public class PasswordResetRequestValidator : AbstractValidator<PasswordResetRequest>
{
    public PasswordResetRequestValidator(PasswordValidator passwordValidator)
    {
        RuleFor(x => x.Password)
            .SetValidator(passwordValidator);
    }
}