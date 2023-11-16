using System.Linq.Expressions;
using FluentResults;
using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.Group;
using HomeworkManager.Model.CustomEntities.User;
using HomeworkManager.Model.Entities;

namespace HomeworkManager.DataAccess.Repositories.Interfaces;

public interface IGroupRepository
{
    Task<bool> ExistsWithNameAsync(int courseId, string groupName, CancellationToken cancellationToken = default);
    Task<Group?> GetByNameAsync(int courseId, string groupName, CancellationToken cancellationToken = default);
    Task<GroupModel?> GetModelAsync(int courseId, string groupName, CancellationToken cancellationToken = default);
    Task<GroupModel?> GetModelAsync(int courseId, string groupName, Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<GroupListRow>> GetAllAsync(int courseId, CancellationToken cancellationToken = default);
    Task<IEnumerable<GroupListRow>> GetAllAsync(int courseId, Guid userId, CancellationToken cancellationToken = default);
    Task<int> GetTeacherCountAsync(int courseId, string groupName, string? searchText = null, CancellationToken cancellationToken = default);
    Task<int> GetStudentCountAsync(int courseId, string groupName, string? searchText = null, CancellationToken cancellationToken = default);

    Task<IEnumerable<UserListRow>> GetTeachersAsync(
        int courseId,
        string groupName,
        PageData? pageData = null,
        CancellationToken cancellationToken = default
    )
    {
        return GetTeachersAsync(courseId, groupName, pageData, u => u.UserId, cancellationToken: cancellationToken);
    }

    Task<IEnumerable<UserListRow>> GetTeachersAsync<TKey>(
        int courseId,
        string groupName,
        PageData? pageData = null,
        Expression<Func<UserListRow, TKey>>? orderBy = null,
        SortDirection sortDirection = SortDirection.Ascending,
        string? searchText = null,
        CancellationToken cancellationToken = default
    );

    Task<IEnumerable<UserListRow>> GetStudentsAsync(
        int courseId,
        string groupName,
        PageData? pageData = null,
        CancellationToken cancellationToken = default
    )
    {
        return GetStudentsAsync(courseId, groupName, pageData, u => u.UserId, cancellationToken: cancellationToken);
    }

    Task<IEnumerable<UserListRow>> GetStudentsAsync<TKey>(
        int courseId,
        string groupName,
        PageData? pageData = null,
        Expression<Func<UserListRow, TKey>>? orderBy = null,
        SortDirection sortDirection = SortDirection.Ascending,
        string? searchText = null,
        CancellationToken cancellationToken = default
    );

    Task<int> CreateAsync(NewGroup newGroup, int courseId, User user, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(int courseId, string groupName, UpdatedGroup updatedGroup, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(int courseId, string groupName, UpdatedGroup updatedGroup, Guid userId, CancellationToken cancellationToken = default);
    Task<Result> AddTeachersAsync(int courseId, string groupName, IEnumerable<Guid> userIds, CancellationToken cancellationToken = default);
    Task<Result> AddStudentsAsync(int courseId, string groupName, IEnumerable<Guid> userIds, CancellationToken cancellationToken = default);
    Task<bool> IsInGroupAsync(int courseId, string groupName, Guid userId, CancellationToken cancellationToken = default);
    Task<bool> IsCreatorAsync(int courseId, string groupName, Guid userId, CancellationToken cancellationToken = default);
    Task<bool> IsTeacherAsync(int courseId, string groupName, Guid userId, CancellationToken cancellationToken = default);
}