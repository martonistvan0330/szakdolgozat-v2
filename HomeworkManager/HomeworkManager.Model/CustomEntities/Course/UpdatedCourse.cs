namespace HomeworkManager.Model.CustomEntities.Course;

public class UpdatedCourse
{
    public int? CourseId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}