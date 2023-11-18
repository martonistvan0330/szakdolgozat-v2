namespace HomeworkManager.Model.CustomEntities.Group;

public class NewGroup
{
    public int? CourseId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}