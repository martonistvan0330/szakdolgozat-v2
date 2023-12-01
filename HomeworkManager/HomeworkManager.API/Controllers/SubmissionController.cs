using FluentValidation;
using HomeworkManager.API.Attributes;
using HomeworkManager.API.Extensions;
using HomeworkManager.API.Validation.Assignment;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.Model.Constants;
using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.Submission;
using Microsoft.AspNetCore.Mvc;

namespace HomeworkManager.API.Controllers;

[HomeworkManagerAuthorize]
[ApiController]
[Route("api/Submission")]
public class SubmissionController : ControllerBase
{
    private readonly AssignmentIdValidator _assignmentIdValidator;
    private readonly ISubmissionManager _submissionManager;
    private readonly IValidator<UpdatedTextSubmission> _updatedTextSubmissionValidator;

    public SubmissionController
    (
        AssignmentIdValidator assignmentIdValidator,
        ISubmissionManager submissionManager,
        IValidator<UpdatedTextSubmission> updatedTextSubmissionValidator
    )
    {
        _assignmentIdValidator = assignmentIdValidator;
        _submissionManager = submissionManager;
        _updatedTextSubmissionValidator = updatedTextSubmissionValidator;
    }

    [HomeworkManagerAuthorize(Roles = $"{Roles.TEACHER},{Roles.ADMINISTRATOR}")]
    [HttpGet("{assignmentId:int}")]
    public async Task<ActionResult<Pageable<SubmissionListRow>>> GetAllByAssignmentAsync(int assignmentId,
        [FromQuery] PageableOptions pageableOptions, CancellationToken cancellationToken)
    {
        var assignmentIdValidationResult = await _assignmentIdValidator.ValidateAsync(
            assignmentId,
            options => { options.IncludeRuleSets("Default", "IsCreator"); },
            cancellationToken);

        if (!assignmentIdValidationResult.IsValid)
        {
            return assignmentIdValidationResult.ToActionResult();
        }

        var getAllResult = await _submissionManager.GetAllByAssignmentAsync(assignmentId, pageableOptions, cancellationToken);

        return getAllResult.ToActionResult();
    }

    [HttpGet("Text/{assignmentId:int}")]
    public async Task<ActionResult<TextSubmissionModel?>> GetTextSubmissionAsync(int assignmentId, Guid? userId, CancellationToken cancellationToken)
    {
        var getResult = await _submissionManager.GetTextSubmissionAsync(assignmentId, userId, cancellationToken);

        return getResult.ToActionResult();
    }

    [HttpGet("File/{assignmentId:int}")]
    public async Task<ActionResult<FileSubmissionModel?>> GetFileSubmissionAsync(int assignmentId, Guid? userId, CancellationToken cancellationToken)
    {
        var getResult = await _submissionManager.GetFileSubmissionAsync(assignmentId, userId, cancellationToken);

        return getResult.ToActionResult();
    }

    [HttpGet("File/{assignmentId:int}/Download")]
    public async Task<ActionResult> DownloadFileSubmissionAsync(int assignmentId, Guid? userId, CancellationToken cancellationToken)
    {
        var downloadResult = await _submissionManager.DownloadFileSubmissionAsync(assignmentId, userId, cancellationToken);

        if (!downloadResult.IsSuccess)
        {
            return downloadResult.ToResult().ToActionResult();
        }

        return File(downloadResult.Value, "application/octet-stream", "submission.txt");
    }

    [HttpPost("Text")]
    public async Task<ActionResult<int>> UpsertTextSubmissionAsync(UpdatedTextSubmission updatedTextSubmission, CancellationToken cancellationToken)
    {
        var updatedTextSubmissionValidationResult = await _updatedTextSubmissionValidator.ValidateAsync(updatedTextSubmission, cancellationToken);

        if (!updatedTextSubmissionValidationResult.IsValid)
        {
            return updatedTextSubmissionValidationResult.ToActionResult();
        }

        var upsertResult = await _submissionManager.UpsertTextSubmissionAsync(updatedTextSubmission, cancellationToken);

        return upsertResult.ToActionResult();
    }

    [HttpPost("File/{assignmentId:int}")]
    public async Task<ActionResult<int>> UploadFileSubmissionAsync(int assignmentId, IFormFile submission, CancellationToken cancellationToken)
    {
        var uploadResult = await _submissionManager.UploadFileSubmissionAsync(assignmentId, submission, cancellationToken);

        return uploadResult.ToActionResult();
    }

    [HttpPatch("{assignmentId:int}")]
    public async Task<ActionResult> SubmitAsync(int assignmentId, CancellationToken cancellationToken = default)
    {
        var submitResult = await _submissionManager.SubmitAsync(assignmentId, cancellationToken);

        return submitResult.ToActionResult();
    }

    [HttpGet("{submissionId:int}/Type")]
    public async Task<ActionResult<int>> GetAssignmentIdAsync(int submissionId, CancellationToken cancellationToken = default)
    {
        var getResult = await _submissionManager.GetAssignmentIdAsync(submissionId, cancellationToken);

        return getResult.ToActionResult();
    }
}