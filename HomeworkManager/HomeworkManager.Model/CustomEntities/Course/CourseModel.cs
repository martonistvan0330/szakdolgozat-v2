namespace HomeworkManager.Model.CustomEntities.Course;

public class CourseModel
{
    public int CourseId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}