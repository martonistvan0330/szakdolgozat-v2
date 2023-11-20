namespace HomeworkManager.Model.Entities;

public class Submission
{
    public int SubmissionId { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public bool IsDraft { get; set; } = true;
    
    public int AssignmentId { get; set; }
    public Assignment Assignment { get; set; } = null!;
    
    public Guid StudentId { get; set; }
    public User Student { get; set; } = null!;
}