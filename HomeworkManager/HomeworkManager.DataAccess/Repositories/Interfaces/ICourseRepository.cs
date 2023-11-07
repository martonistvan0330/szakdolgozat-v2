using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.Course;
using HomeworkManager.Model.Entities;
using HomeworkManager.Model.ErrorEntities;

namespace HomeworkManager.DataAccess.Repositories.Interfaces;

public interface ICourseRepository
{
    Task<bool> ExistsAsync(int courseId);
    Task<bool> ExistsWithNameAsync(string name);
    Task<IEnumerable<CourseCard>> GetAllAsync();
    Task<IEnumerable<CourseCard>> GetAllByUserAsync(Guid userId);
    Task<CourseModel?> GetModelAsync(int courseId);
    Task<CourseModel?> GetModelByUserAsync(int courseId, Guid userId);
    Task<Result<int, BusinessError>> CreateAsync(NewCourse newCourse, User user);
    Task<BusinessError?> UpdateAsync(int courseId, UpdateCourse updatedCourse, User? user = null);
    Task<bool> IsInCourseAsync(int courseId, Guid userId);
    Task<bool> IsCreatorAsync(int courseId, Guid userId);
    Task<bool> IsTeacherAsync(int courseId, Guid userId);
}