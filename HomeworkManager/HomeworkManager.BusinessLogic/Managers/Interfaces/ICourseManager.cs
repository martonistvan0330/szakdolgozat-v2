using FluentResults;
using HomeworkManager.Model.CustomEntities.Course;
using HomeworkManager.Model.CustomEntities.User;

namespace HomeworkManager.BusinessLogic.Managers.Interfaces;

public interface ICourseManager
{
    Task<bool> ExistsWithIdAsync(int courseId, CancellationToken cancellationToken = default);
    Task<bool> NameAvailableAsync(UpdatedCourse updatedCourse, CancellationToken cancellationToken = default);
    Task<bool> NameAvailableAsync(string name, CancellationToken cancellationToken = default);
    Task<Result<CourseModel>> GetModelAsync(int courseId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<CourseListRow>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<UserListRow>>> GetTeachersAsync(int courseId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<UserListRow>>> GetStudentsAsync(int courseId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<UserListRow>>> GetAddableTeachersAsync(int courseId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<UserListRow>>> GetAddableStudentsAsync(int courseId, CancellationToken cancellationToken = default);
    Task<Result<int>> CreateAsync(NewCourse newCourse, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(int courseId, UpdatedCourse updatedCourse, CancellationToken cancellationToken = default);
    Task<Result> AddTeachersAsync(int courseId, ICollection<Guid> userIds, CancellationToken cancellationToken = default);
    Task<Result> AddStudentsAsync(int courseId, ICollection<Guid> userIds, CancellationToken cancellationToken = default);
    Task<bool> IsInCourseAsync(int courseId, CancellationToken cancellationToken = default);
    Task<bool> IsCreatorAsync(int courseId, CancellationToken cancellationToken = default);
    Task<bool> IsTeacherAsync(int courseId, CancellationToken cancellationToken = default);
}