using FluentResults;
using HomeworkManager.Model.CustomEntities.Appointment;

namespace HomeworkManager.BusinessLogic.Managers.Interfaces;

public interface IAppointmentManager
{
    Task<Result<IEnumerable<AppointmentRow>>> GetAllByAssignmentAsync(int assignmentId, CancellationToken cancellationToken = default);
    Task<Result> CreateAsync(NewAppointment newAppointment, CancellationToken cancellationToken = default);
    Task<Result<int>> SignUpAsync(int appointmentId, CancellationToken cancellationToken = default);
    Task<bool> AvailableAsync(NewAppointment newAppointment, CancellationToken cancellationToken = default);
}