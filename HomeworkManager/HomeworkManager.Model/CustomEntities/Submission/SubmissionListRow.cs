namespace HomeworkManager.Model.CustomEntities.Submission;

public class SubmissionListRow
{
    public required int SubmissionId { get; set; }
    public required Guid StudentId { get; set; }
    public required string StudentName { get; set; }
    public required DateTime SubmittedAt { get; set; }
}