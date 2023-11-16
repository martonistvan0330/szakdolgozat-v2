using FluentValidation;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.Model.CustomEntities.Group;

namespace HomeworkManager.API.Validation.Group;

public class UpdatedGroupValidator : AbstractValidator<UpdatedGroup>
{
    public UpdatedGroupValidator(int courseId, string groupName, IGroupManager groupManager)
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty();
            
        RuleFor(x => x)
            .MustAsync(async (updatedGroup, cancellationToken) =>
                await groupManager.NameAvailableAsync(courseId, updatedGroup.Name, cancellationToken));
    }
}