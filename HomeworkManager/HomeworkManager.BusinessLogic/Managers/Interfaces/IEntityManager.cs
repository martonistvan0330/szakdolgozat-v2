using HomeworkManager.Model.CustomEntities.Entity;

namespace HomeworkManager.BusinessLogic.Managers.Interfaces;

public interface IEntityManager
{
    Task<EntityModel?> GetOrNullAsync(int entityId);
    Task<IEnumerable<EntityModel>> GetAllAsync();
    Task<int> CreateAsync(EntityModel entityModel);
    Task DeleteAsync(int entityId);
}