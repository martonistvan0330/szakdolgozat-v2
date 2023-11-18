namespace HomeworkManager.Model.CustomEntities.Group;

public class UpdatedGroup
{
    public int? CourseId { get; set; }
    public string? OldName { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}