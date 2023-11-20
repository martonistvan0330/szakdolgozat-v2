using System.Linq.Expressions;
using FluentResults;
using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.Submission;

namespace HomeworkManager.DataAccess.Repositories.Interfaces;

public interface ISubmissionRepository
{
    Task<int> GetCountByAssignmentIdAsync(int assignmentId, string? searchText = null,
        CancellationToken cancellationToken = default);
    
    Task<IEnumerable<SubmissionListRow>> GetAllByAssignmentIdAsync(
        int assignmentId,
        PageData? pageData = null,
        string? searchText = null,
        CancellationToken cancellationToken = default
    )
    {
        return GetAllByAssignmentIdAsync(assignmentId, pageData, s => s.SubmittedAt, searchText: searchText, cancellationToken: cancellationToken);
    }

    Task<IEnumerable<SubmissionListRow>> GetAllByAssignmentIdAsync<TKey>(
        int assignmentId,
        PageData? pageData = null,
        Expression<Func<SubmissionListRow, TKey>>? orderBy = null,
        SortDirection sortDirection = SortDirection.Ascending,
        string? searchText = null,
        CancellationToken cancellationToken = default
    );
    
    Task<TextSubmissionModel?> GetTextSubmissionAsync(int assignmentId, Guid userId, CancellationToken cancellationToken = default);
    Task<int> UpsertTextSubmissionAsync(UpdatedTextSubmission updatedTextSubmission, Guid userId, CancellationToken cancellationToken = default);
    Task<Result> SubmitAsync(int assignmentId, Guid userId, CancellationToken cancellationToken = default);
    Task<int?> GetAssignmentIdAsync(int submissionId, CancellationToken cancellationToken = default);
}