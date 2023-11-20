namespace HomeworkManager.Model.CustomEntities.Submission;

public class SubmissionModel
{
    public required int SubmissionId { get; set; }
    public required string StudentName { get; set; }
    public required string? SubmittedAt { get; set; }
    public required bool IsDraft { get; set; }
}