using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.CustomEntities.Entity;

namespace HomeworkManager.BusinessLogic.Managers;

public class EntityManager : IEntityManager
{
    private readonly IEntityRepository _entityRepository;

    public EntityManager(IEntityRepository entityRepository)
    {
        _entityRepository = entityRepository;
    }

    public async Task<EntityModel?> GetOrNullAsync(int entityId)
    {
        return await _entityRepository.GetOrNullAsync(entityId);
    }

    public async Task<IEnumerable<EntityModel>> GetAllAsync()
    {
        return await _entityRepository.GetAllAsync();
    }

    public async Task<int> CreateAsync(EntityModel entityModel)
    {
        return await _entityRepository.CreateAsync(entityModel);
    }

    public async Task DeleteAsync(int entityId)
    {
        await _entityRepository.DeleteAsync(entityId);
    }
}