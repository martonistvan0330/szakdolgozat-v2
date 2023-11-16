using FluentValidation;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.Model.CustomEntities.Group;

namespace HomeworkManager.API.Validation.Group;

public class NewGroupValidator : AbstractValidator<NewGroup>
{
    public NewGroupValidator(int courseId, IGroupManager groupManager)
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .MustAsync(async (groupName, cancellationToken) =>
                await groupManager.NameAvailableAsync(courseId, groupName, cancellationToken));
    }
}