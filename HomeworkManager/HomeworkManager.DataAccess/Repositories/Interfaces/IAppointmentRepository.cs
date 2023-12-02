using FluentResults;
using HomeworkManager.Model.CustomEntities.Appointment;

namespace HomeworkManager.DataAccess.Repositories.Interfaces;

public interface IAppointmentRepository
{
    Task<IEnumerable<AppointmentRow>> GetAllByAssignmentIdAsync(int assignmentId, Guid currentUserId, CancellationToken cancellationToken = default);

    Task CreateAsync(
        int assignmentId,
        DateTime date,
        IEnumerable<int> appointmentTimes,
        Guid teacherId,
        CancellationToken cancellationToken = default
    );

    Task<Result<int>> SignUpStudentAsync(int appointmentId, Guid studentId, CancellationToken cancellationToken = default);

    Task<bool> ExistsBetweenAsync(
        int assignmentId,
        DateTime date,
        int startTime,
        int endTime,
        Guid teacherId,
        CancellationToken cancellationToken = default
    );

    Task AssignAllStudentsAsync(int assignmentId, CancellationToken cancellationToken = default);
    Task AssignStudentsWithSubmissionAsync(int assignmentId, CancellationToken cancellationToken = default);
}