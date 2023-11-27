namespace HomeworkManager.DataAccess.Repositories.Interfaces;

public interface IAppointmentRepository
{
    Task CreateAsync(
        int assignmentId,
        DateTime date,
        IEnumerable<int> appointmentTimes,
        Guid teacherId,
        CancellationToken cancellationToken = default
    );
    
    Task<bool> ExistsBetweenAsync(
        int assignmentId,
        DateTime date,
        int startTime,
        int endTime,
        Guid teacherId,
        CancellationToken cancellationToken = default
    );
}