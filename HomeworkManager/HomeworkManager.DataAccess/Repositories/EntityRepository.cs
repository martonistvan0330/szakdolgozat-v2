using HomeworkManager.DataAccess.Repositories.Extensions;
using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.Contexts;
using HomeworkManager.Model.CustomEntities.Enitity;
using HomeworkManager.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomeworkManager.DataAccess.Repositories
{
    public class EntityRepository : IEntityRepository
    {
        private readonly HomeworkManagerContext _context;

        public EntityRepository(HomeworkManagerContext context)
        {
            _context = context;
        }

        public async Task<EntityModel?> GetOrNullAsync(int entityId)
        {
            return await _context.Entities.Where(e => e.EntityId == entityId).ToModels().SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<EntityModel>> GetAllAsync()
        {
            return await _context.Entities.ToModels().ToListAsync();
        }

        public async Task<int> CreateAsync(EntityModel entityModel)
        {
            var dbEntity = new Entity
            {
                EntityId = 0,
                Name = entityModel.Name
            };

            _context.Entities.Add(dbEntity);
            await _context.SaveChangesAsync();

            return dbEntity.EntityId;
        }

        public async Task DeleteAsync(int entityId)
        {
            var dbEntity = _context.Entities.SingleOrDefault(e => e.EntityId == entityId);

            if (dbEntity is not null)
            {
                _context.Entities.Remove(dbEntity);
                await _context.SaveChangesAsync();
            }
        }
    }
}