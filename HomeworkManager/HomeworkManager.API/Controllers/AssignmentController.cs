using FluentValidation;
using HomeworkManager.API.Attributes;
using HomeworkManager.API.Extensions;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.Model.Constants;
using HomeworkManager.Model.CustomEntities.Assignment;
using Microsoft.AspNetCore.Mvc;

namespace HomeworkManager.API.Controllers;

[HomeworkManagerAuthorize]
[ApiController]
[Route("api/Assignment")]
public class AssignmentController : ControllerBase
{
    private readonly IAssignmentManager _assignmentManager;
    private readonly AbstractValidator<NewAssignment> _newAssignmentValidator;

    public AssignmentController(IAssignmentManager assignmentManager, AbstractValidator<NewAssignment> newAssignmentValidator)
    {
        _assignmentManager = assignmentManager;
        _newAssignmentValidator = newAssignmentValidator;
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

    [HttpGet("NameAvailable")]
    public async Task<ActionResult<bool>> NameAvailableAsync([FromQuery] NewAssignment newAssignment, CancellationToken cancellationToken)
    {
        return await _assignmentManager.NameAvailableAsync(newAssignment, cancellationToken);
    }
}