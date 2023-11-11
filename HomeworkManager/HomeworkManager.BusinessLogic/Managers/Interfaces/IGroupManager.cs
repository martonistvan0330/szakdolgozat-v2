using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.Group;
using HomeworkManager.Model.CustomEntities.User;
using HomeworkManager.Model.ErrorEntities;

namespace HomeworkManager.BusinessLogic.Managers.Interfaces;

public interface IGroupManager
{
    Task<Result<IEnumerable<GroupListRow>, BusinessError>> GetAllByUserAsync(int courseId, string? username);
    Task<bool> ExistsAsync(string groupName, int courseId);
    Task<Result<GroupModel?, BusinessError>> GetModelByUserAsync(int courseId, string groupName, string? username);
    Task<Result<Pageable<UserListRow>, BusinessError>> GetTeachersAsync(int courseId, string groupName, string? username, PageableOptions options);
    Task<Result<Pageable<UserListRow>, BusinessError>> GetStudentsAsync(int courseId, string groupName, string? username, PageableOptions options);
    Task<Result<IEnumerable<UserListRow>, BusinessError>> GetAddableTeachersAsync(int courseId, string groupName, string? username);
    Task<Result<IEnumerable<UserListRow>, BusinessError>> GetAddableStudentsAsync(int courseId, string groupName, string? username);
    Task<Result<int, BusinessError>> CreateAsync(NewGroup newGroup, int courseId, string? username);
    Task<BusinessError?> UpdateAsync(int courseId, string groupName, UpdateGroup updatedGroup, string? username);
    Task<BusinessError?> AddTeachersAsync(int courseId, string groupName, string? username, ICollection<Guid> userIds);
    Task<BusinessError?> AddStudentsAsync(int courseId, string groupName, string? username, ICollection<Guid> userIds);
    Task<bool> IsInGroupAsync(string groupName, int courseId, string? username);
    Task<bool> IsCreatorAsync(string groupName, int courseId, string? username);
    Task<bool> IsTeacherAsync(string groupName, int courseId, string? username);
    Task<bool> NameAvailableAsync(string name, int courseId);
}