using FluentResults;
using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.Group;
using HomeworkManager.Model.CustomEntities.User;

namespace HomeworkManager.BusinessLogic.Managers.Interfaces;

public interface IGroupManager
{
    Task<bool> ExistsWithNameAsync(int courseId, string groupName, CancellationToken cancellationToken = default);
    Task<bool> NameAvailableAsync(int courseId, string name, CancellationToken cancellationToken = default);
    Task<bool> NameAvailableAsync(int courseId, string groupName, string name, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<GroupListRow>>> GetAllAsync(int courseId, CancellationToken cancellationToken = default);
    Task<Result<GroupModel?>> GetModelAsync(int courseId, string groupName, CancellationToken cancellationToken = default);

    Task<Result<Pageable<UserListRow>>> GetTeachersAsync(int courseId, string groupName, PageableOptions options,
        CancellationToken cancellationToken = default);

    Task<Result<Pageable<UserListRow>>> GetStudentsAsync(int courseId, string groupName, PageableOptions options,
        CancellationToken cancellationToken = default);

    Task<Result<IEnumerable<UserListRow>>> GetAddableTeachersAsync(int courseId, string groupName, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<UserListRow>>> GetAddableStudentsAsync(int courseId, string groupName, CancellationToken cancellationToken = default);
    Task<Result<int>> CreateAsync(NewGroup newGroup, int courseId, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(int courseId, string groupName, UpdatedGroup updatedGroup, CancellationToken cancellationToken = default);
    Task<Result> AddTeachersAsync(int courseId, string groupName, IEnumerable<Guid> userIds, CancellationToken cancellationToken = default);
    Task<Result> AddStudentsAsync(int courseId, string groupName, IEnumerable<Guid> userIds, CancellationToken cancellationToken = default);
    Task<Result> RemoveTeacherAsync(int courseId, string groupName, Guid teacherId, CancellationToken cancellationToken = default);
    Task<Result> RemoveStudentAsync(int courseId, string groupName, Guid studentId, CancellationToken cancellationToken = default);
    Task<bool> IsInGroupAsync(int courseId, string groupName, CancellationToken cancellationToken = default);
    Task<bool> IsCreatorAsync(int courseId, string groupName, CancellationToken cancellationToken = default);
    Task<bool> IsTeacherAsync(int courseId, string groupName, CancellationToken cancellationToken = default);
}