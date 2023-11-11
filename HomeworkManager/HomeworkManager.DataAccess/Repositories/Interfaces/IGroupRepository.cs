using System.Linq.Expressions;
using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.Group;
using HomeworkManager.Model.CustomEntities.User;
using HomeworkManager.Model.Entities;
using HomeworkManager.Model.ErrorEntities;

namespace HomeworkManager.DataAccess.Repositories.Interfaces;

public interface IGroupRepository
{
    Task<bool> ExistsWithNameAsync(int courseId, string groupName);
    Task<IEnumerable<GroupListRow>> GetAllAsync(int courseId);
    Task<IEnumerable<GroupListRow>> GetAllByUserAsync(int courseId, Guid userId);
    Task<GroupModel?> GetModelAsync(int courseId, string groupName);
    Task<GroupModel?> GetModelByUserAsync(int courseId, string groupName, Guid userId);
    Task<int> GetTeacherCountAsync(int courseId, string groupName, string? searchText = null);
    Task<int> GetStudentCountAsync(int courseId, string groupName, string? searchText = null);

    Task<IEnumerable<UserListRow>> GetTeachersAsync(
        int courseId,
        string groupName,
        PageData? pageData = null
    )
    {
        return GetTeachersAsync(courseId, groupName, pageData, u => u.UserId);
    }

    Task<IEnumerable<UserListRow>> GetTeachersAsync<TKey>(
        int courseId,
        string groupName,
        PageData? pageData = null,
        Expression<Func<UserListRow, TKey>>? orderBy = null,
        SortDirection sortDirection = SortDirection.Ascending,
        string? searchText = null
    );

    Task<IEnumerable<UserListRow>> GetStudentsAsync(
        int courseId,
        string groupName,
        PageData? pageData = null
    )
    {
        return GetStudentsAsync(courseId, groupName, pageData, u => u.UserId);
    }

    Task<IEnumerable<UserListRow>> GetStudentsAsync<TKey>(
        int courseId,
        string groupName,
        PageData? pageData = null,
        Expression<Func<UserListRow, TKey>>? orderBy = null,
        SortDirection sortDirection = SortDirection.Ascending,
        string? searchText = null
    );

    Task<Result<int, BusinessError>> CreateAsync(NewGroup newGroup, int courseId, User user);
    Task<BusinessError?> UpdateAsync(int courseId, string groupName, UpdateGroup updatedGroup, User? user = null);
    Task AddTeachersAsync(int courseId, string groupName, ICollection<Guid> userIds);
    Task AddStudentsAsync(int courseId, string groupName, ICollection<Guid> userIds);
    Task<bool> IsInGroupAsync(int courseId, string groupName, Guid userId);
    Task<bool> IsCreatorAsync(int courseId, string groupName, Guid userId);
    Task<bool> IsTeacherAsync(int courseId, string groupName, Guid userId);
}