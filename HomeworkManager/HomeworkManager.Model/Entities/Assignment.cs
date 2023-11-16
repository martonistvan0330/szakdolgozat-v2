using HomeworkManager.Model.Constants;

namespace HomeworkManager.Model.Entities;

public class Assignment
{
    public int AssignmentId { get; set; }
    public required string Name { get; set; }
    public bool IsDraft { get; set; } = true;

    public AssignmentTypeId? AssignmentTypeId { get; set; }
    public AssignmentType? AssignmentType { get; set; }

    public int GroupId { get; set; }
    public Group Group { get; set; } = null!;

    public Guid CreatorId { get; set; }
    public User Creator { get; set; } = null!;
}