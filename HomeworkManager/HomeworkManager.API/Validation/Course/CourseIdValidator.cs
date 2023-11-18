using FluentValidation;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.BusinessLogic.Services.Authentication.Interfaces;
using HomeworkManager.Model.Constants;
using HomeworkManager.Model.Constants.Errors;
using HomeworkManager.Model.Constants.Errors.Course;

namespace HomeworkManager.API.Validation.Course;

public class CourseIdValidator : AbstractValidator<int>
{
    public CourseIdValidator(ICourseManager courseManager, ICurrentUserService currentUserService)
    {
        RuleFor(x => x)
            .MustAsync(async (courseId, cancellationToken) =>
                await courseManager.ExistsWithIdAsync(courseId, cancellationToken))
            .WithMessage(CourseErrorMessages.COURSE_WITH_ID_NOT_FOUND);

        RuleFor(x => x)
            .MustAsync(async (courseId, cancellationToken) =>
            {
                if (await currentUserService.HasRoleAsync(Roles.ADMINISTRATOR, cancellationToken))
                {
                    return true;
                }

                return await courseManager.IsInCourseAsync(courseId, cancellationToken);
            })
            .WithErrorCode(ErrorCodes.FORBIDDEN);

        RuleSet("IsTeacher", () =>
        {
            RuleFor(x => x)
                .MustAsync(async (courseId, cancellationToken) =>
                {
                    if (await currentUserService.HasRoleAsync(Roles.ADMINISTRATOR, cancellationToken))
                    {
                        return true;
                    }

                    return await courseManager.IsTeacherAsync(courseId, cancellationToken);
                })
                .WithErrorCode(ErrorCodes.FORBIDDEN);
        });

        RuleSet("IsCreator", () =>
        {
            RuleFor(x => x)
                .MustAsync(async (courseId, cancellationToken) =>
                {
                    if (await currentUserService.HasRoleAsync(Roles.ADMINISTRATOR, cancellationToken))
                    {
                        return true;
                    }

                    return await courseManager.IsCreatorAsync(courseId, cancellationToken);
                })
                .WithErrorCode(ErrorCodes.FORBIDDEN);
        });
    }
}