using HomeworkManager.Model.CustomEntities.Entity;

namespace HomeworkManager.Model.Entities;

public class Entity
{
    public int EntityId { get; set; }
    public required string Name { get; set; }

    public EntityModel ToModel()
    {
        return new EntityModel
        {
            Name = Name
        };
    }
}