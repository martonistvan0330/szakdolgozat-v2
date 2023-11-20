using FluentResults;
using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.Submission;

namespace HomeworkManager.BusinessLogic.Managers.Interfaces;

public interface ISubmissionManager
{
    Task<Result<Pageable<SubmissionListRow>>> GetAllByAssignmentAsync(int assignmentId, PageableOptions options, CancellationToken cancellationToken = default);
    Task<Result<TextSubmissionModel?>> GetTextSubmissionAsync(int assignmentId, Guid? userId, CancellationToken cancellationToken = default);
    Task<Result<int>> UpsertTextSubmissionAsync(UpdatedTextSubmission updatedTextSubmission, CancellationToken cancellationToken = default);
    Task<Result> SubmitAsync(int assignmentId, CancellationToken cancellationToken = default);
    Task<Result<int>> GetAssignmentIdAsync(int submissionId, CancellationToken cancellationToken = default);
}