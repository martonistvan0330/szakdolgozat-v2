using FluentValidation;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.Model.CustomEntities.Group;

namespace HomeworkManager.API.Validation.Group;

public class UpdatedGroupValidator : AbstractValidator<UpdatedGroup>
{
    public UpdatedGroupValidator(IGroupManager groupManager)
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x)
            .MustAsync(async (updatedGroup, cancellationToken) =>
                await groupManager.NameAvailableAsync(updatedGroup, cancellationToken));
    }
}