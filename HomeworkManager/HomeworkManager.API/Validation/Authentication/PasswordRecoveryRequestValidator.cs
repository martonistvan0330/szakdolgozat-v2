using FluentValidation;
using HomeworkManager.Model.CustomEntities.Authentication;

namespace HomeworkManager.API.Validation.Authentication;

public class PasswordRecoveryRequestValidator : AbstractValidator<PasswordRecoveryRequest>
{
    public PasswordRecoveryRequestValidator()
    {
        
    }
}