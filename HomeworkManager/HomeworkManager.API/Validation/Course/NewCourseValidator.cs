using FluentValidation;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.Model.CustomEntities.Course;

namespace HomeworkManager.API.Validation.Course;

public class NewCourseValidator : AbstractValidator<NewCourse>
{
    public NewCourseValidator(ICourseManager courseManager)
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .MustAsync(async (courseName, cancellationToken) =>
                await courseManager.NameAvailableAsync(courseName, cancellationToken));
    }
}