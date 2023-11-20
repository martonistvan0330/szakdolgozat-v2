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

    [HttpPost("File")]
    public async Task<ActionResult<int>> UpsertFileSubmissionAsync(UpdatedFileSubmission updatedTextSubmission, CancellationToken cancellationToken)
    {
        return 0;
        // var upsertResult = await _submissionManager.UpsertTextSubmissionAsync(updatedTextSubmission, cancellationToken);
        //
        // return upsertResult.ToActionResult();
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