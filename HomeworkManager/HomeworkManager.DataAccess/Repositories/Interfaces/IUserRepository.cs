using System.Collections;
using System.Linq.Expressions;
using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.User;
using HomeworkManager.Model.Entities;

namespace HomeworkManager.DataAccess.Repositories.Interfaces;

public interface IUserRepository
{
    Task<bool> ExistsByIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<int> GetCountAsync(string? searchText = null, CancellationToken cancellationToken = default);
    Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<Guid?> GetIdByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<UserModel?> GetModelByIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<UserModel?> GetModelByUsernameAsync(string username, CancellationToken cancellationToken = default);

    Task<IEnumerable<UserListRow>> GetAllModelsAsync(PageData? pageData = null, CancellationToken cancellationToken = default)
    {
        return GetAllModelsAsync(pageData, u => u.UserId, cancellationToken: cancellationToken);
    }

    Task<IEnumerable<UserListRow>> GetAllModelsAsync<TKey>
    (
        PageData? pageData = null,
        Expression<Func<UserListRow, TKey>>? orderBy = null,
        SortDirection sortDirection = SortDirection.Ascending,
        string? searchText = null,
        CancellationToken cancellationToken = default
    );

    Task<IEnumerable<string>> GetRoleNamesToAdd(Guid userId, IEnumerable<int> roleIds, CancellationToken cancellationToken = default);
    Task<IEnumerable<string>> GetRoleNamesToRemove(Guid userId, IEnumerable<int> roleIds, CancellationToken cancellationToken = default);
    Task<bool> ConfirmEmailAsync(Guid userId, CancellationToken cancellationToken = default);
}