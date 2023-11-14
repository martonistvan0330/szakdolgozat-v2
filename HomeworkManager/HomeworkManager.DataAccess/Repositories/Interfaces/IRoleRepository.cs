using HomeworkManager.Model.CustomEntities.Role;

namespace HomeworkManager.DataAccess.Repositories.Interfaces;

public interface IRoleRepository
{
    Task<bool> ExistsByIdAsync(int roleId, CancellationToken cancellationToken = default);
    Task<IEnumerable<RoleModel>> GetAllAsync(CancellationToken cancellationToken = default);
}