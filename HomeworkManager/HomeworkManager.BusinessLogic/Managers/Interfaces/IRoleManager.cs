using HomeworkManager.Model.CustomEntities.Role;

namespace HomeworkManager.BusinessLogic.Managers.Interfaces;

public interface IRoleManager
{
    Task<bool> ExistsByIdAsync(int roleId, CancellationToken cancellationToken = default);
    Task<IEnumerable<RoleModel>> GetAllAsync(CancellationToken cancellationToken = default);
}