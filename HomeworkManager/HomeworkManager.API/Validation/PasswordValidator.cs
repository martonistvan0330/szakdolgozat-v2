using FluentValidation;

namespace HomeworkManager.API.Validation;

public class PasswordValidator : AbstractValidator<string>
{
    public PasswordValidator()
    {
        RuleFor(x => x)
            .MinimumLength(6)
            .Must(password =>
                password.Count(char.IsLower) > 0
                && password.Count(char.IsUpper) > 0
                && password.Count(char.IsDigit) > 0);
    }
}