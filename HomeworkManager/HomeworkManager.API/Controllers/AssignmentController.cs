using FluentValidation;
using HomeworkManager.API.Attributes;
using HomeworkManager.API.Extensions;
using HomeworkManager.API.Validation.Assignment;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.Model.Constants;
using HomeworkManager.Model.CustomEntities.Assignment;
using HomeworkManager.Model.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HomeworkManager.API.Controllers;

[HomeworkManagerAuthorize]
[ApiController]
[Route("api/Assignment")]
public class AssignmentController : ControllerBase
{
    private readonly AssignmentIdValidator _assignmentIdValidator;
    private readonly IAssignmentManager _assignmentManager;
    private readonly IValidator<NewAssignment> _newAssignmentValidator;
    private readonly IValidator<UpdatedAssignment> _updatedAssignmentValidator;

    public AssignmentController
    (
        AssignmentIdValidator assignmentIdValidator,
        IAssignmentManager assignmentManager,
        IValidator<NewAssignment> newAssignmentValidator,
        IValidator<UpdatedAssignment> updatedAssignmentValidator
    )
    {
        _assignmentIdValidator = assignmentIdValidator;
        _assignmentManager = assignmentManager;
        _newAssignmentValidator = newAssignmentValidator;
        _updatedAssignmentValidator = updatedAssignmentValidator;
    }

    [HttpGet("{assignmentId:int}")]
    public async Task<ActionResult<AssignmentModel>> GetAsync(int assignmentId, CancellationToken cancellationToken)
    {
        var assignmentIdValidationResult = await _assignmentIdValidator.ValidateAsync(assignmentId, cancellationToken);

        if (!assignmentIdValidationResult.IsValid)
        {
            return assignmentIdValidationResult.ToActionResult();
        }

        var getResult = await _assignmentManager.GetModelAsync(assignmentId, cancellationToken);

        return getResult.ToActionResult();
    }

    [HomeworkManagerAuthorize(Roles = $"{Roles.TEACHER},{Roles.ADMINISTRATOR}")]
    [HttpPost]
    public async Task<ActionResult<int>> CreateAsync(NewAssignment newAssignment, CancellationToken cancellationToken)
    {
        var newAssignmentValidationResult = await _newAssignmentValidator.ValidateAsync(newAssignment, cancellationToken);

        if (!newAssignmentValidationResult.IsValid)
        {
            return newAssignmentValidationResult.ToActionResult();
        }

        var createResult = await _assignmentManager.CreateAsync(newAssignment, cancellationToken);

        return createResult.ToActionResult();
    }

    [HomeworkManagerAuthorize(Roles = $"{Roles.TEACHER},{Roles.ADMINISTRATOR}")]
    [HttpPut("{assignmentId:int}")]
    public async Task<ActionResult<int>> UpdateAsync(int assignmentId, UpdatedAssignment updatedAssignment, CancellationToken cancellationToken)
    {
        var assignmentIdValidationResult = await _assignmentIdValidator.ValidateAsync(
            assignmentId,
            options => { options.IncludeRuleSets("Default", "IsDraft", "IsCreator"); },
            cancellationToken);

        if (!assignmentIdValidationResult.IsValid)
        {
            return assignmentIdValidationResult.ToActionResult();
        }

        var updatedAssignmentValidationResult = await _updatedAssignmentValidator.ValidateAsync(updatedAssignment, cancellationToken);

        if (!updatedAssignmentValidationResult.IsValid)
        {
            return updatedAssignmentValidationResult.ToActionResult();
        }

        var updateResult = await _assignmentManager.UpdateAsync(assignmentId, updatedAssignment, cancellationToken);

        return updateResult.ToActionResult();
    }

    [HomeworkManagerAuthorize(Roles = $"{Roles.TEACHER},{Roles.ADMINISTRATOR}")]
    [HttpPatch("Publish/{assignmentId:int}")]
    public async Task<ActionResult<int>> PublishAsync(int assignmentId, CancellationToken cancellationToken)
    {
        var assignmentIdValidationResult = await _assignmentIdValidator.ValidateAsync(
            assignmentId,
            options => { options.IncludeRuleSets("Default", "IsDraft", "IsCreator"); },
            cancellationToken);

        if (!assignmentIdValidationResult.IsValid)
        {
            return assignmentIdValidationResult.ToActionResult();
        }

        var publishResult = await _assignmentManager.PublishAsync(assignmentId, cancellationToken);

        return publishResult.ToActionResult();
    }

    [HttpGet("{assignmentId:int}/IsInGroup")]
    public async Task<ActionResult<bool>> IsInGroupAsync(int assignmentId, CancellationToken cancellationToken)
    {
        var assignmentIdValidationResult = await _assignmentIdValidator.ValidateAsync(assignmentId, cancellationToken);

        if (!assignmentIdValidationResult.IsValid)
        {
            return assignmentIdValidationResult.ToActionResult();
        }

        return await _assignmentManager.IsInGroupAsync(assignmentId, cancellationToken);
    }

    [HttpGet("{assignmentId:int}/IsCreator")]
    public async Task<ActionResult<bool>> IsCreatorAsync(int assignmentId, CancellationToken cancellationToken)
    {
        var assignmentIdValidationResult = await _assignmentIdValidator.ValidateAsync(assignmentId, cancellationToken);

        if (!assignmentIdValidationResult.IsValid)
        {
            return assignmentIdValidationResult.ToActionResult();
        }

        return await _assignmentManager.IsCreatorAsync(assignmentId, cancellationToken);
    }

    [HttpGet("{assignmentId:int}/IsTeacher")]
    public async Task<ActionResult<bool>> IsTeacherAsync(int assignmentId, CancellationToken cancellationToken)
    {
        var assignmentIdValidationResult = await _assignmentIdValidator.ValidateAsync(assignmentId, cancellationToken);

        if (!assignmentIdValidationResult.IsValid)
        {
            return assignmentIdValidationResult.ToActionResult();
        }

        return await _assignmentManager.IsTeacherAsync(assignmentId, cancellationToken);
    }

    [HttpGet("NameAvailable")]
    public async Task<ActionResult<bool>> NameAvailableAsync([FromQuery] NewAssignment newAssignment, CancellationToken cancellationToken)
    {
        return await _assignmentManager.NameAvailableAsync(newAssignment, cancellationToken);
    }

    [HttpGet("NameAvailable/{assignmentId:int}")]
    public async Task<ActionResult<bool>> UpdatedNameAvailableAsync(int assignmentId, string name, CancellationToken cancellationToken)
    {
        return await _assignmentManager.NameAvailableAsync(assignmentId, name, cancellationToken);
    }

    [HttpGet("Types")]
    public async Task<ActionResult<IEnumerable<AssignmentType>>> GetAssignmentTypesAsync(CancellationToken cancellationToken)
    {
        return Ok(await _assignmentManager.GetAssignmentTypes(cancellationToken));
    }
}