using FluentValidation;
using HomeworkManager.API.Validation.Course;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.Model.Constants.Errors.Assignment;
using HomeworkManager.Model.CustomEntities.Assignment;
using HomeworkManager.Model.CustomEntities.Group;

namespace HomeworkManager.API.Validation.Assignment;

public class NewAssignmentValidator : AbstractValidator<NewAssignment>
{
    public NewAssignmentValidator(CourseIdValidator courseIdValidator, IValidator<GroupInfo> groupNameValidator, IAssignmentManager assignmentManager)
    {
        RuleFor(x => x)
            .MustAsync(async (newAssignment, cancellationToken) =>
                await assignmentManager.NameAvailableAsync(newAssignment, cancellationToken))
            .WithMessage(AssignmentErrorMessages.ASSIGNMENT_NAME_NOT_AVAILABLE);

        RuleFor(x => x.GroupInfo.CourseId)
            .SetValidator(courseIdValidator);

        RuleFor(x => x.GroupInfo)
            .SetValidator(groupNameValidator, "Default", "IsTeacher");
    }
}