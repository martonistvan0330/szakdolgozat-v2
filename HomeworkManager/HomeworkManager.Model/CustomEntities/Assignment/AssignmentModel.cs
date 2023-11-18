using HomeworkManager.Model.Constants;

namespace HomeworkManager.Model.CustomEntities.Assignment;

public class AssignmentModel
{
    public required int AssignmentId { get; set; }
    public required string Name { get; set; }
    public required string? Description { get; set; }
    public required DateTime Deadline { get; set; }
    public required bool PresentationRequired { get; set; }
    public required bool IsDraft { get; set; }
    public required AssignmentTypeId? AssignmentTypeId { get; set; }
    public required string? AssignmentTypeName { get; set; }
    public required int CourseId { get; set; }
    public required string CourseName { get; set; }
    public required string GroupName { get; set; }
}