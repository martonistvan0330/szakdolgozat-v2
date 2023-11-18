namespace HomeworkManager.Model.CustomEntities.Assignment;

public class AssignmentListRow
{
    public required int AssignmentId { get; set; }
    public required string Name { get; set; }
    public required DateTimeOffset Deadline { get; set; }
    public required bool IsDraft { get; set; }
}