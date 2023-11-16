using FluentResults;
using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.Authentication;
using HomeworkManager.Model.CustomEntities.User;

namespace HomeworkManager.BusinessLogic.Managers.Interfaces;

public interface IUserManager
{
    Task<bool> ExistsByIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> UsernameAvailableAsync(string username, CancellationToken cancellationToken = default);
    Task<bool> EmailAvailableAsync(string email, CancellationToken cancellationToken = default);
    Task<UserModel> GetCurrentModelAsync(CancellationToken cancellationToken = default);
    Task<Result<UserModel>> GetModelByIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Result<UserModel>> GetModelByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<Result<UserModel>> GetModelByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Pageable<UserListRow>> GetAllAsync(PageableOptions options, CancellationToken cancellationToken = default);
    Task<Result<UserModel>> CheckPasswordAsync(AuthenticationRequest authenticationRequest, CancellationToken cancellationToken = default);
    Task<Result<UserModel>> CreateAsync(NewUser newUser, CancellationToken cancellationToken = default);
    Task<Result> UpdatePasswordAsync(Guid userId, string password, CancellationToken cancellationToken = default);
    Task<Result> ConfirmEmailAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Result> UpdateRoles(Guid userId, ICollection<int> roleIds, CancellationToken cancellationToken = default);
}