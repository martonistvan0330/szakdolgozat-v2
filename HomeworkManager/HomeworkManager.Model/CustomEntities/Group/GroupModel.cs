namespace HomeworkManager.Model.CustomEntities.Group;

public class GroupModel
{
    public int GroupId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}