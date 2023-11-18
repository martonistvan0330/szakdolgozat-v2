using FluentValidation;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.Model.CustomEntities.Course;

namespace HomeworkManager.API.Validation.Course;

public class UpdatedCourseValidator : AbstractValidator<UpdatedCourse>
{
    public UpdatedCourseValidator(ICourseManager courseManager)
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x)
            .MustAsync(async (updatedCourse, cancellationToken) =>
                await courseManager.NameAvailableAsync(updatedCourse, cancellationToken));
    }
}