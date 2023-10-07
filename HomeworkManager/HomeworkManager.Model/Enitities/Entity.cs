using HomeworkManager.Model.CustomEntities.Enitity;

namespace HomeworkManager.Model.Enitities
{
    public class Entity
    {
        public int EntityId { get; set; }
        public required string Name { get; set; }

        public EntityModel ToModel()
        {
            return new EntityModel
            {
                Name = Name,
            };
        }
    }
}
