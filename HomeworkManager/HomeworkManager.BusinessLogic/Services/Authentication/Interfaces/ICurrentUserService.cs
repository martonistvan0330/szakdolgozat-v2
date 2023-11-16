using HomeworkManager.Model.CustomEntities.User;
using HomeworkManager.Model.Entities;

namespace HomeworkManager.BusinessLogic.Services.Authentication.Interfaces;

public interface ICurrentUserService
{
    Task<Guid> GetIdAsync(CancellationToken cancellationToken = default);
    Task<User> GetAsync(CancellationToken cancellationToken = default);
    Task<UserModel> GetModelAsync(CancellationToken cancellationToken = default);
    Task<bool> HasRoleAsync(string role, CancellationToken cancellationToken = default);
}