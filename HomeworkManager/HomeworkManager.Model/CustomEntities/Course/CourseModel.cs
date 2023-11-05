using HomeworkManager.Model.CustomEntities.Group;

namespace HomeworkManager.Model.CustomEntities.Course;

public class CourseModel
{
    public int CourseId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public ICollection<GroupListRow> Groups { get; set; } = new HashSet<GroupListRow>();
}