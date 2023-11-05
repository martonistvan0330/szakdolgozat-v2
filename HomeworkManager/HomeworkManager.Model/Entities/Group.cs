namespace HomeworkManager.Model.Entities;

public class Group
{
    public int GroupId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    
    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;
    public Guid CreatorId { get; set; }
    public User Creator { get; set; } = null!;

    public ICollection<User> Teachers { get; set; } = new HashSet<User>();
    public ICollection<User> Students { get; set; } = new HashSet<User>();
}