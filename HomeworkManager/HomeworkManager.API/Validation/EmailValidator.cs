using FluentValidation;

namespace HomeworkManager.API.Validation;

public class EmailValidator : AbstractValidator<string>
{
    public EmailValidator()
    {
        RuleFor(x => x)
            .NotNull()
            .NotEmpty()
            .EmailAddress();
    }
}