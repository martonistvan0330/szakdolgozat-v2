using FluentValidation;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.Model.Constants;
using HomeworkManager.Model.Constants.Errors;
using HomeworkManager.Model.Constants.Errors.Group;

namespace HomeworkManager.API.Validation.Group;

public class GroupNameValidator : AbstractValidator<string>
{
    public GroupNameValidator(int courseId, IGroupManager groupManager, IUserManager userManager)
    {
        RuleFor(x => x)
            .MustAsync(async (groupName, cancellationToken) =>
                await groupManager.ExistsWithNameAsync(courseId, groupName, cancellationToken))
            .WithMessage(GroupErrorMessages.GROUP_WITH_NAME_NOT_FOUND);
        
        RuleFor(x => x)
            .MustAsync(async (groupName, cancellationToken) =>
            {
                if (await userManager.CurrentUserHasRoleAsync(Roles.ADMINISTRATOR, cancellationToken))
                {
                    return true;
                }

                return await groupManager.IsInGroupAsync(courseId, groupName, cancellationToken);
            })
            .WithErrorCode(ErrorCodes.FORBIDDEN);
        
        RuleSet("IsTeacher", () =>
        {
            RuleFor(x => x)
                .MustAsync(async (groupName, cancellationToken) =>
                {
                    if (await userManager.CurrentUserHasRoleAsync(Roles.ADMINISTRATOR, cancellationToken))
                    {
                        return true;
                    }

                    return await groupManager.IsTeacherAsync(courseId, groupName, cancellationToken);
                })
                .WithErrorCode(ErrorCodes.FORBIDDEN);
        });
        
        RuleSet("IsCreator", () =>
        {
            RuleFor(x => x)
                .MustAsync(async (groupName, cancellationToken) =>
                {
                    if (await userManager.CurrentUserHasRoleAsync(Roles.ADMINISTRATOR, cancellationToken))
                    {
                        return true;
                    }

                    return await groupManager.IsCreatorAsync(courseId, groupName, cancellationToken);
                })
                .WithErrorCode(ErrorCodes.FORBIDDEN);
        });
    }
}