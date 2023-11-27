using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.Contexts;
using HomeworkManager.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomeworkManager.DataAccess.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly HomeworkManagerContext _context;

    public AppointmentRepository(HomeworkManagerContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(int assignmentId, DateTime date, IEnumerable<int> appointmentTimes, Guid teacherId,
        CancellationToken cancellationToken = default)
    {
        foreach (int appointmentTime in appointmentTimes)
        {
            Appointment appointment = new()
            {
                Date = date,
                TimeInMinutes = appointmentTime,
                AssignmentId = assignmentId,
                TeacherId = teacherId
            };

            _context.Appointments.Add(appointment);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsBetweenAsync(
        int assignmentId, DateTime date, int startTime, int endTime,
        Guid teacherId, CancellationToken cancellationToken = default
    )
    {
        return await _context.Appointments
            .Where(a => a.AssignmentId == assignmentId
                        && a.Date.Date == date.Date && a.TeacherId == teacherId)
            .Where(a => startTime <= a.TimeInMinutes && a.TimeInMinutes <= endTime)
            .AnyAsync(cancellationToken);
    }
}