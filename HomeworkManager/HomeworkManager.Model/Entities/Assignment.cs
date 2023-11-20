using HomeworkManager.Model.Constants;

namespace HomeworkManager.Model.Entities;

public class Assignment
{
    public int AssignmentId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required DateTime Deadline { get; set; }
    public bool PresentationRequired { get; set; } = false;
    public bool IsDraft { get; set; } = true;

    public AssignmentTypeId? AssignmentTypeId { get; set; }
    public AssignmentType? AssignmentType { get; set; }

    public int GroupId { get; set; }
    public Group Group { get; set; } = null!;

    public Guid CreatorId { get; set; }
    public User Creator { get; set; } = null!;

    public ICollection<Submission> Submissions { get; set; } = new HashSet<Submission>();
}