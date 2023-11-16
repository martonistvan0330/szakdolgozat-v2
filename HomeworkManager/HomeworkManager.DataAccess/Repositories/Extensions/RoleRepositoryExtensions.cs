using HomeworkManager.Model.CustomEntities.Role;
using HomeworkManager.Model.Entities;

namespace HomeworkManager.DataAccess.Repositories.Extensions;

public static class RoleRepositoryExtensions
{
    public static IQueryable<RoleModel> ToModel(this IQueryable<Role> roles)
    {
        return roles
            .Select(r => new RoleModel
            {
                RoleId = r.RoleId,
                Name = r.Name!
            });
    }
}