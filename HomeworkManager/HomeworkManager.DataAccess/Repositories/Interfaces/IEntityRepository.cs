using HomeworkManager.Model.CustomEntities.Entity;

namespace HomeworkManager.DataAccess.Repositories.Interfaces;

public interface IEntityRepository
{
    Task<EntityModel?> GetOrNullAsync(int entityId);
    Task<IEnumerable<EntityModel>> GetAllAsync();
    Task<int> CreateAsync(EntityModel entityModel);
    Task DeleteAsync(int entityId);
}