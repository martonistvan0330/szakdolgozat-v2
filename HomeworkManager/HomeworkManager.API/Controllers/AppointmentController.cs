using FluentValidation;
using HomeworkManager.API.Attributes;
using HomeworkManager.API.Extensions;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.Model.CustomEntities.Appointment;
using Microsoft.AspNetCore.Mvc;

namespace HomeworkManager.API.Controllers;

[HomeworkManagerAuthorize]
[ApiController]
[Route("api/Appointment")]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentManager _appointmentManager;
    private readonly IValidator<NewAppointment> _newAppointmentValidator;

    public AppointmentController(IAppointmentManager appointmentManager, IValidator<NewAppointment> newAppointmentValidator)
    {
        _appointmentManager = appointmentManager;
        _newAppointmentValidator = newAppointmentValidator;
    }

    [HttpGet("{assignmentId:int}")]
    public async Task<ActionResult<IEnumerable<AppointmentRow>>> GetAllByAssignmentAsync(int assignmentId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
        // var getResult = await _appointmentManager.GetAllByAssignmentIdAsync(assignmentId, cancellationToken);
        //
        // return getResult.ToActionResult();
    }

    [HttpPatch("{appointmentId:int}")]
    public async Task<ActionResult> SignUpAsync(int appointmentId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
        // var signUpResult = await _appointmentManager.SignUpAsync(appointmentId, cancellationToken);
        //
        // return signUpResult.ToActionResult();
    }

    [HttpPost]
    public async Task<ActionResult> CreateAppointmentsAsync(NewAppointment newAppointment, CancellationToken cancellationToken)
    {
        var newAppointmentValidationResult = await _newAppointmentValidator.ValidateAsync(newAppointment, cancellationToken);

        if (!newAppointmentValidationResult.IsValid)
        {
            return newAppointmentValidationResult.ToActionResult();
        }

        var createResult = await _appointmentManager.CreateAsync(newAppointment, cancellationToken);

        return createResult.ToActionResult();
    }
}