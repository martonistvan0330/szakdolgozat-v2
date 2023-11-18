using HomeworkManager.Model.Constants;

namespace HomeworkManager.Model.CustomEntities.Assignment;

public class UpdatedAssignment
{
    public required int AssignmentId { get; set; }
    public required string Name { get; set; }
    public required string? Description { get; set; }
    public required DateTime Deadline { get; set; }
    public required AssignmentTypeId AssignmentTypeId { get; set; }
    public required bool PresentationRequired { get; set; }
}