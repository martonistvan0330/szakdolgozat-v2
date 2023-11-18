using FluentValidation;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.Model.CustomEntities.Group;

namespace HomeworkManager.API.Validation.Group;

public class NewGroupValidator : AbstractValidator<NewGroup>
{
    public NewGroupValidator(IGroupManager groupManager)
    {
        RuleFor(x => x)
            .NotNull()
            .NotEmpty()
            .MustAsync(async (newGroup, cancellationToken) =>
                await groupManager.NameAvailableAsync(newGroup, cancellationToken));
    }
}