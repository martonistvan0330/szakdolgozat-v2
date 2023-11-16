using FluentResults;
using HomeworkManager.Model.CustomEntities.Course;
using HomeworkManager.Model.CustomEntities.User;
using HomeworkManager.Model.Entities;

namespace HomeworkManager.DataAccess.Repositories.Interfaces;

public interface ICourseRepository
{
    Task<bool> ExistsWithIdAsync(int courseId, CancellationToken cancellationToken = default);
    Task<bool> ExistsWithNameAsync(string name, CancellationToken cancellationToken = default);
    Task<string?> GetNameByIdAsync(int courseId, CancellationToken cancellationToken = default);
    Task<Course?> GetByIdAsync(int courseId, CancellationToken cancellationToken = default);
    Task<CourseModel?> GetModelAsync(int courseId, CancellationToken cancellationToken = default);
    Task<CourseModel?> GetModelIdAsync(int courseId, Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CourseCard>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<CourseCard>> GetAllAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserListRow>> GetTeachersAsync(int courseId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserListRow>> GetStudentsAsync(int courseId, CancellationToken cancellationToken = default);
    Task<int> CreateAsync(NewCourse newCourse, User user, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(int courseId, UpdatedCourse updatedCourse, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(int courseId, UpdatedCourse updatedCourse, Guid userId, CancellationToken cancellationToken = default);
    Task<Result> AddTeachersAsync(int courseId, IEnumerable<Guid> userIds, CancellationToken cancellationToken = default);
    Task<Result> AddStudentsAsync(int courseId, IEnumerable<Guid> userIds, CancellationToken cancellationToken = default);
    Task<bool> IsInCourseAsync(int courseId, Guid userId, CancellationToken cancellationToken = default);
    Task<bool> IsCreatorAsync(int courseId, Guid userId, CancellationToken cancellationToken = default);
    Task<bool> IsTeacherAsync(int courseId, Guid userId, CancellationToken cancellationToken = default);
}