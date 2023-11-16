using FluentValidation;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.Model.CustomEntities.Course;

namespace HomeworkManager.API.Validation.Course;

public class UpdatedCourseValidator : AbstractValidator<UpdatedCourse>
{
    public UpdatedCourseValidator(int courseId, ICourseManager courseManager)
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty();
            
        RuleFor(x => x)
            .MustAsync(async (updatedCourse, cancellationToken) =>
                await courseManager.NameAvailableAsync(courseId, updatedCourse.Name, cancellationToken));
    }
}