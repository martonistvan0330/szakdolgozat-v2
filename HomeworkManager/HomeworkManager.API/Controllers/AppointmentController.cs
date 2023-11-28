using FluentValidation;
using HomeworkManager.API.Attributes;
using HomeworkManager.API.Extensions;
using HomeworkManager.API.Hubs;
using HomeworkManager.API.Hubs.ClientInterfaces;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.Model.Constants;
using HomeworkManager.Model.CustomEntities.Appointment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HomeworkManager.API.Controllers;

[HomeworkManagerAuthorize]
[ApiController]
[Route("api/Appointment")]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentManager _appointmentManager;
    private readonly IHubContext<AppointmentHub, IAppointmentHubClient> _hubContext;
    private readonly IValidator<NewAppointment> _newAppointmentValidator;

    public AppointmentController
    (
        IAppointmentManager appointmentManager,
        IHubContext<AppointmentHub, IAppointmentHubClient> hubContext,
        IValidator<NewAppointment> newAppointmentValidator
    )
    {
        _appointmentManager = appointmentManager;
        _hubContext = hubContext;
        _newAppointmentValidator = newAppointmentValidator;
    }

    [HttpGet("{assignmentId:int}")]
    public async Task<ActionResult<IEnumerable<AppointmentRow>>> GetAllByAssignmentAsync(int assignmentId, CancellationToken cancellationToken)
    {
        var getResult = await _appointmentManager.GetAllByAssignmentAsync(assignmentId, cancellationToken);

        return getResult.ToActionResult();
    }

    [HomeworkManagerAuthorize(Roles.TEACHER, Roles.ADMINISTRATOR)]
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

    [HttpPatch("{appointmentId:int}")]
    public async Task<ActionResult<int>> SignUpAsync(int appointmentId, CancellationToken cancellationToken)
    {
        var signUpResult = await _appointmentManager.SignUpAsync(appointmentId, cancellationToken);

        if (signUpResult.IsSuccess)
        {
            await _hubContext.Clients.Group($"Assignment_{signUpResult.Value}").Refresh();
        }

        return signUpResult.ToActionResult();
    }
}