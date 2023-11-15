using FluentValidation;
using HomeworkManager.Model.CustomEntities.Authentication;

namespace HomeworkManager.API.Validation.Authentication;

public class RefreshRequestValidator : AbstractValidator<RefreshRequest>
{
    public RefreshRequestValidator()
    {
        
    }
}