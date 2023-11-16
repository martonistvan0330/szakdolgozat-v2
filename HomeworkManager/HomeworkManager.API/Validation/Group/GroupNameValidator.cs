using FluentValidation;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.BusinessLogic.Services.Authentication.Interfaces;
using HomeworkManager.Model.Constants;
using HomeworkManager.Model.Constants.Errors;
using HomeworkManager.Model.Constants.Errors.Group;

namespace HomeworkManager.API.Validation.Group;

public class GroupNameValidator : AbstractValidator<string>
{
    public int CourseId { get; set; }
    
    public GroupNameValidator(IGroupManager groupManager, ICurrentUserService currentUserService)
    {
        RuleFor(x => x)
            .MustAsync(async (groupName, cancellationToken) =>
                await groupManager.ExistsWithNameAsync(CourseId, groupName, cancellationToken))
            .WithMessage(GroupErrorMessages.GROUP_WITH_NAME_NOT_FOUND);
        
        RuleFor(x => x)
            .MustAsync(async (groupName, cancellationToken) =>
            {
                if (await currentUserService.HasRoleAsync(Roles.ADMINISTRATOR, cancellationToken))
                {
                    return true;
                }

                return await groupManager.IsInGroupAsync(CourseId, groupName, cancellationToken);
            })
            .WithErrorCode(ErrorCodes.FORBIDDEN);
        
        RuleSet("IsTeacher", () =>
        {
            RuleFor(x => x)
                .MustAsync(async (groupName, cancellationToken) =>
                {
                    if (await currentUserService.HasRoleAsync(Roles.ADMINISTRATOR, cancellationToken))
                    {
                        return true;
                    }

                    return await groupManager.IsTeacherAsync(CourseId, groupName, cancellationToken);
                })
                .WithErrorCode(ErrorCodes.FORBIDDEN);
        });
        
        RuleSet("IsCreator", () =>
        {
            RuleFor(x => x)
                .MustAsync(async (groupName, cancellationToken) =>
                {
                    if (await currentUserService.HasRoleAsync(Roles.ADMINISTRATOR, cancellationToken))
                    {
                        return true;
                    }

                    return await groupManager.IsCreatorAsync(CourseId, groupName, cancellationToken);
                })
                .WithErrorCode(ErrorCodes.FORBIDDEN);
        });
    }
}