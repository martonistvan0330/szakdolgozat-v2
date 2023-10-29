using HomeworkManager.Model.CustomEntities.Entity;
using HomeworkManager.Model.Entities;

namespace HomeworkManager.DataAccess.Repositories.Extensions;

public static class EntityRepositoryExtensions
{
    public static IQueryable<EntityModel> ToModels(this IQueryable<Entity> entities)
    {
        return entities.Select(e => e.ToModel());
    }
}