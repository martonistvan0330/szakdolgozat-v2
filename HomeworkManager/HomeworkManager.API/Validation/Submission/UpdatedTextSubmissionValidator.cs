using FluentValidation;
using HomeworkManager.API.Validation.Assignment;
using HomeworkManager.Model.CustomEntities.Submission;

namespace HomeworkManager.API.Validation.Submission;

public class UpdatedTextSubmissionValidator : AbstractValidator<UpdatedTextSubmission>
{
    public UpdatedTextSubmissionValidator(AssignmentIdValidator assignmentIdValidator)
    {
        RuleFor(x => x.AssignmentId)
            .SetValidator(assignmentIdValidator);

        RuleFor(x => x.Answer)
            .NotNull()
            .NotEmpty();
    }
}