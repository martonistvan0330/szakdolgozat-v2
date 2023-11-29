using FluentResults;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.BusinessLogic.Services.Authentication.Interfaces;
using HomeworkManager.BusinessLogic.Services.File.Interfaces;
using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.Constants.Errors.Submission;
using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.Errors;
using HomeworkManager.Model.CustomEntities.Submission;
using Microsoft.AspNetCore.Http;

namespace HomeworkManager.BusinessLogic.Managers;

public class SubmissionManager : ISubmissionManager
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IFileService _fileService;
    private readonly ISubmissionRepository _submissionRepository;

    public SubmissionManager
    (
        ICurrentUserService currentUserService,
        IFileService fileService,
        ISubmissionRepository submissionRepository
    )
    {
        _currentUserService = currentUserService;
        _fileService = fileService;
        _submissionRepository = submissionRepository;
    }

    public async Task<Result<Pageable<SubmissionListRow>>> GetAllByAssignmentAsync(int assignmentId,
        PageableOptions options, CancellationToken cancellationToken = default)
    {
        int submissionCount = await _submissionRepository.GetCountByAssignmentIdAsync(assignmentId, options.SearchText, cancellationToken);

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

    public async Task<Result<FileSubmissionModel?>> GetFileSubmissionAsync(int assignmentId, Guid? userId,
        CancellationToken cancellationToken = default)
    {
        userId ??= await _currentUserService.GetIdAsync(cancellationToken);

        return await _submissionRepository.GetFileSubmissionAsync(assignmentId, userId.Value, cancellationToken);
    }

    public async Task<Result<Stream>> DownloadFileSubmissionAsync(int assignmentId, Guid? studentId, CancellationToken cancellationToken = default)
    {
        studentId ??= await _currentUserService.GetIdAsync(cancellationToken);

        var fileSubmission = await _submissionRepository.GetFileSubmissionAsync(assignmentId, studentId.Value, cancellationToken);

        if (fileSubmission is null)
        {
            return new NotFoundError(SubmissionErrorMessages.SUBMISSION_WITH_ID_NOT_FOUND);
        }

        return await _fileService.DownloadSubmissionAsync(assignmentId, studentId.Value, fileSubmission.FileName, cancellationToken);
    }

    public async Task<Result<int>> UpsertTextSubmissionAsync(UpdatedTextSubmission updatedTextSubmission,
        CancellationToken cancellationToken = default)
    {
        var userId = await _currentUserService.GetIdAsync(cancellationToken);

        return await _submissionRepository.UpsertTextSubmissionAsync(updatedTextSubmission, userId, cancellationToken);
    }

    public async Task<Result<int>> UploadFileSubmissionAsync(int assignmentId, IFormFile submission, CancellationToken cancellationToken = default)
    {
        var currentUserId = await _currentUserService.GetIdAsync(cancellationToken);

        string fileName = await _fileService.UploadSubmissionAsync(assignmentId, submission, currentUserId, cancellationToken);

        return await _submissionRepository.UpsertFileSubmissionAsync(assignmentId, currentUserId, fileName, cancellationToken);
    }

    public async Task<Result> SubmitAsync(int assignmentId, CancellationToken cancellationToken = default)
    {
        var userId = await _currentUserService.GetIdAsync(cancellationToken);

        return await _submissionRepository.SubmitAsync(assignmentId, userId, cancellationToken);
    }

    public async Task<Result<int>> GetAssignmentIdAsync(int submissionId, CancellationToken cancellationToken = default)
    {
        int? assignmentId = await _submissionRepository.GetAssignmentIdAsync(submissionId, cancellationToken);

        if (assignmentId is null)
        {
            return new BusinessError(SubmissionErrorMessages.SUBMISSION_WITH_ID_NOT_FOUND);
        }

        return assignmentId;
    }
}