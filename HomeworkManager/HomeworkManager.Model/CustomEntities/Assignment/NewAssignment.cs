using HomeworkManager.Model.CustomEntities.Group;

namespace HomeworkManager.Model.CustomEntities.Assignment;

public class NewAssignment
{
    public required string Name { get; set; }
    public required GroupInfo GroupInfo { get; set; }
}