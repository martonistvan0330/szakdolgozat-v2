using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.CustomEntities.Role;

namespace HomeworkManager.BusinessLogic.Managers;

public class RoleManager : IRoleManager
{
    private readonly IRoleRepository _roleRepository;

    public RoleManager(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<bool> ExistsByIdAsync(int roleId, CancellationToken cancellationToken = default)
    {
        return await _roleRepository.ExistsByIdAsync(roleId, cancellationToken);
    }

    public async Task<IEnumerable<RoleModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _roleRepository.GetAllAsync(cancellationToken);
    }
}