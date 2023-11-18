using FluentValidation;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.Model.CustomEntities.Assignment;

namespace HomeworkManager.API.Validation.Assignment;

public class UpdatedAssignmentValidator : AbstractValidator<UpdatedAssignment>
{
    public UpdatedAssignmentValidator(IAssignmentManager assignmentManager)
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x)
            .MustAsync(async (updatedAssignment, cancellationToken) =>
                await assignmentManager.NameAvailableAsync(updatedAssignment.AssignmentId, updatedAssignment.Name, cancellationToken));
    }
}