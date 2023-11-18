using FluentValidation;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.BusinessLogic.Services.Authentication.Interfaces;
using HomeworkManager.Model.Constants;
using HomeworkManager.Model.Constants.Errors;
using HomeworkManager.Model.Constants.Errors.Assignment;

namespace HomeworkManager.API.Validation.Assignment;

public class AssignmentIdValidator : AbstractValidator<int>
{
    public AssignmentIdValidator(IAssignmentManager assignmentManager, ICurrentUserService currentUserService)
    {
        RuleFor(x => x)
            .MustAsync(async (assignmentId, cancellationToken) =>
                await assignmentManager.ExistsWithIdAsync(assignmentId, cancellationToken))
            .WithMessage(AssignmentErrorMessages.ASSIGNMENT_WITH_ID_NOT_FOUND);

        RuleSet("IsInGroup", () =>
        {
            RuleFor(x => x)
                .MustAsync(async (assignmentId, cancellationToken) =>
                {
                    if (await currentUserService.HasRoleAsync(Roles.ADMINISTRATOR, cancellationToken))
                    {
                        return true;
                    }

                    return await assignmentManager.IsInGroupAsync(assignmentId, cancellationToken);
                })
                .WithErrorCode(ErrorCodes.FORBIDDEN);
        });

        RuleSet("IsDraft", () =>
        {
            RuleFor(x => x)
                .MustAsync(async (assignmentId, cancellationToken) =>
                    await assignmentManager.ExistsDraftWithIdAsync(assignmentId, cancellationToken));
        });

        RuleSet("IsTeacher", () =>
        {
            RuleFor(x => x)
                .MustAsync(async (assignmentId, cancellationToken) =>
                {
                    if (await currentUserService.HasRoleAsync(Roles.ADMINISTRATOR, cancellationToken))
                    {
                        return true;
                    }

                    return await assignmentManager.IsTeacherAsync(assignmentId, cancellationToken);
                })
                .WithErrorCode(ErrorCodes.FORBIDDEN);
        });

        RuleSet("IsCreator", () =>
        {
            RuleFor(x => x)
                .MustAsync(async (assignmentId, cancellationToken) =>
                {
                    if (await currentUserService.HasRoleAsync(Roles.ADMINISTRATOR, cancellationToken))
                    {
                        return true;
                    }

                    return await assignmentManager.IsCreatorAsync(assignmentId, cancellationToken);
                })
                .WithErrorCode(ErrorCodes.FORBIDDEN);
        });
    }
}