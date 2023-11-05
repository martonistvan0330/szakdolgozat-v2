namespace HomeworkManager.Model.Entities;

public class ManagedCourse
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;
}