using FluentValidation;
using HomeworkManager.API.Validation.Course;
using HomeworkManager.API.Validation.Group;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.Model.Constants.Errors.Assignment;
using HomeworkManager.Model.CustomEntities.Assignment;
using HomeworkManager.Model.CustomEntities.Group;

namespace HomeworkManager.API.Validation.Assignment;

public class NewAssignmentValidator : AbstractValidator<NewAssignment>
{
    public NewAssignmentValidator(CourseIdValidator courseIdValidator, AbstractValidator<GroupName> groupNameValidator, IAssignmentManager assignmentManager)
    {
        RuleFor(x => x)
            .MustAsync(async (newAssignment, cancellationToken) =>
                await assignmentManager.NameAvailableAsync(newAssignment, cancellationToken))
            .WithMessage(AssignmentErrorMessages.ASSIGNMENT_NAME_NOT_AVAILABLE);

        RuleFor(x => x.Group.CourseId)
            .SetValidator(courseIdValidator);

        RuleFor(x => x.Group)
            .SetValidator(groupNameValidator, "Default", "IsTeacher");
    }
}