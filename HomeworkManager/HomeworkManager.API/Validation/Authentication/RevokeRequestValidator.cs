using FluentValidation;
using HomeworkManager.Model.CustomEntities.Authentication;

namespace HomeworkManager.API.Validation.Authentication;

public class RevokeRequestValidator : AbstractValidator<RevokeRequest>
{
    public RevokeRequestValidator()
    {
        
    }
}