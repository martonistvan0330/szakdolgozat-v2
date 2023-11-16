using HomeworkManager.DataAccess.Repositories.Extensions;
using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.Contexts;
using HomeworkManager.Model.CustomEntities.Role;
using Microsoft.EntityFrameworkCore;

namespace HomeworkManager.DataAccess.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly HomeworkManagerContext _context;

    public RoleRepository(HomeworkManagerContext context)
    {
        _context = context;
    }

    public Task<bool> ExistsByIdAsync(int roleId, CancellationToken cancellationToken = default)
    {
        return _context.Roles
            .Where(r => r.RoleId == roleId)
            .AnyAsync(cancellationToken);
    }

    public async Task<IEnumerable<RoleModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Roles
            .ToModel()
            .ToListAsync(cancellationToken);
    }
}