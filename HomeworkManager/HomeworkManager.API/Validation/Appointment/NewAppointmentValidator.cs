using FluentValidation;
using HomeworkManager.API.Validation.Assignment;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.Model.Constants.Errors.Appointment;
using HomeworkManager.Model.CustomEntities.Appointment;

namespace HomeworkManager.API.Validation.Appointment;

public class NewAppointmentValidator : AbstractValidator<NewAppointment>
{
    public NewAppointmentValidator(AssignmentIdValidator assignmentIdValidator, IAppointmentManager appointmentManager)
    {
        RuleFor(x => x.AssignmentId)
            .SetValidator(assignmentIdValidator);

        RuleFor(x => x)
            .MustAsync(async (newAppointment, cancellationToken) => 
                await appointmentManager.AvailableAsync(newAppointment, cancellationToken))
            .WithMessage(AppointmentErrorMessages.APPOINTMENT_CREATE_CONFLICT);
    }
}