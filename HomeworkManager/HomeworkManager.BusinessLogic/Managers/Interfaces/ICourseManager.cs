using System.Collections;
using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.Course;
using HomeworkManager.Model.CustomEntities.User;
using HomeworkManager.Model.ErrorEntities;

namespace HomeworkManager.BusinessLogic.Managers.Interfaces;

public interface ICourseManager
{
    Task<Result<IEnumerable<CourseCard>, BusinessError>> GetAllCoursesByUserAsync(string? username);
    Task<bool> ExistsAsync(int courseId);
    Task<Result<CourseModel?, BusinessError>> GetModelByUserAsync(int courseId, string? username);
    Task<Result<IEnumerable<UserListRow>, BusinessError>> GetTeachersAsync(int courseId, string? username);
    Task<Result<IEnumerable<UserListRow>, BusinessError>> GetStudentsAsync(int courseId, string? username);
    Task<Result<IEnumerable<UserListRow>, BusinessError>> GetAddableTeachersAsync(int courseId, string? username);
    Task<Result<IEnumerable<UserListRow>, BusinessError>> GetAddableStudentsAsync(int courseId, string? username);
    Task<Result<int, BusinessError>> CreateAsync(NewCourse newCourse, string? username);
    Task<BusinessError?> UpdateAsync(int courseId, UpdateCourse updatedCourse, string? username);
    Task<BusinessError?> AddTeachersAsync(int courseId, string? username, IEnumerable<Guid> userIds);
    Task<BusinessError?> AddStudentsAsync(int courseId, string? username, IEnumerable<Guid> userIds);
    Task<bool> IsInCourseAsync(int courseId, string? username);
    Task<bool> IsCreatorAsync(int courseId, string? username);
    Task<bool> IsTeacherAsync(int courseId, string? username);
    Task<bool> NameAvailableAsync(string name);
}