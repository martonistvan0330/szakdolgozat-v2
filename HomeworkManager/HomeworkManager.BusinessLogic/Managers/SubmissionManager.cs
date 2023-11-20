using FluentResults;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.BusinessLogic.Services.Authentication.Interfaces;
using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.Constants.Errors.Submission;
using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.Errors;
using HomeworkManager.Model.CustomEntities.Submission;

namespace HomeworkManager.BusinessLogic.Managers;

public class SubmissionManager : ISubmissionManager
{
    private readonly ICurrentUserService _currentUserService;
    private readonly ISubmissionRepository _submissionRepository;

    public SubmissionManager(ICurrentUserService currentUserService, ISubmissionRepository submissionRepository)
    {
        _currentUserService = currentUserService;
        _submissionRepository = submissionRepository;
    }

    public async Task<Result<Pageable<SubmissionListRow>>> GetAllByAssignmentAsync(int assignmentId, 
        PageableOptions options, CancellationToken cancellationToken = default)
    {
        var submissionCount = await _submissionRepository.GetCountByAssignmentIdAsync(assignmentId, options.SearchText, cancellationToken);

        var submissions = options.SortOptions?.Sort switch
        {
            "studentName" => await _submissionRepository.GetAllByAssignmentIdAsync(
                assignmentId,
                options.PageData,
                s => s.StudentName,
                options.SortOptions.SortDirection.ToSortDirection(),
                options.SearchText,
                cancellationToken),
            "submittedAt" => await _submissionRepository.GetAllByAssignmentIdAsync(
                assignmentId,
                options.PageData,
                s => s.SubmittedAt,
                options.SortOptions.SortDirection.ToSortDirection(),
                options.SearchText,
                cancellationToken),
            _ => await _submissionRepository.GetAllByAssignmentIdAsync(assignmentId, options.PageData, options.SearchText, cancellationToken)
        };

        return new Pageable<SubmissionListRow>
        {
            Items = submissions,
            TotalCount = submissionCount
        };
    }

    public async Task<Result<TextSubmissionModel?>> GetTextSubmissionAsync(int assignmentId, Guid? userId,
        CancellationToken cancellationToken = default)
    {
        userId ??= await _currentUserService.GetIdAsync(cancellationToken);

        return await _submissionRepository.GetTextSubmissionAsync(assignmentId, userId.Value, cancellationToken);
    }

    public async Task<Result<int>> UpsertTextSubmissionAsync(UpdatedTextSubmission updatedTextSubmission,
        CancellationToken cancellationToken = default)
    {
        var userId = await _currentUserService.GetIdAsync(cancellationToken);

        return await _submissionRepository.UpsertTextSubmissionAsync(updatedTextSubmission, userId, cancellationToken);
    }

    public async Task<Result> SubmitAsync(int assignmentId, CancellationToken cancellationToken = default)
    {
        var userId = await _currentUserService.GetIdAsync(cancellationToken);

        return await _submissionRepository.SubmitAsync(assignmentId, userId, cancellationToken);
    }

    public async Task<Result<int>> GetAssignmentIdAsync(int submissionId, CancellationToken cancellationToken = default)
    {
        var assignmentId = await _submissionRepository.GetAssignmentIdAsync(submissionId, cancellationToken);

        if (assignmentId is null)
        {
            return new BusinessError(SubmissionErrorMessages.SUBMISSION_WITH_ID_NOT_FOUND);
        }

        return assignmentId;
    }
}