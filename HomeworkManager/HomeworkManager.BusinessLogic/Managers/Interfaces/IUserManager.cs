using System.Collections;
using FluentResults;
using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.User;

namespace HomeworkManager.BusinessLogic.Managers.Interfaces;

public interface IUserManager
{
    Task<bool> ExistsByIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> UsernameAvailableAsync(string username, CancellationToken cancellationToken = default);
    Task<bool> EmailAvailableAsync(string email, CancellationToken cancellationToken = default);
    Task<Guid> GetCurrentUserIdAsync(CancellationToken cancellationToken = default);
    Task<UserModel> GetCurrentUserModelAsync(CancellationToken cancellationToken = default);
    Task<Result<UserModel>> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Result<UserModel>> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<Pageable<UserListRow>> GetAllAsync(PageableOptions options, CancellationToken cancellationToken = default);
    Task<bool> CurrentUserHasRoleAsync(string role, CancellationToken cancellationToken = default);
    Task<Result> UpdateRoles(Guid userId, ICollection<int> roleIds, CancellationToken cancellationToken = default);
}