using FluentResults;
using HomeworkManager.Model.CustomEntities.Appointment;

namespace HomeworkManager.BusinessLogic.Managers.Interfaces;

public interface IAppointmentManager
{
    Task<Result> CreateAsync(NewAppointment newAppointment, CancellationToken cancellationToken = default);
    Task<bool> AvailableAsync(NewAppointment newAppointment, CancellationToken cancellationToken = default);
}