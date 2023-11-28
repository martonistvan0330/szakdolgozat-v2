using FluentResults;
using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.Constants.Errors.Appointment;
using HomeworkManager.Model.Contexts;
using HomeworkManager.Model.CustomEntities.Appointment;
using HomeworkManager.Model.CustomEntities.Errors;
using HomeworkManager.Model.Entities;
using HomeworkManager.Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace HomeworkManager.DataAccess.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly HomeworkManagerContext _context;

    public AppointmentRepository(HomeworkManagerContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AppointmentRow>> GetAllByAssignmentIdAsync(int assignmentId, Guid currentUserId,
        CancellationToken cancellationToken = default)
    {
        bool hasAny = await _context.Appointments
            .Where(a => a.AssignmentId == assignmentId)
            .AnyAsync(cancellationToken);

        if (!hasAny)
        {
            return Array.Empty<AppointmentRow>();
        }

        var minDate = await _context.Appointments
            .Where(a => a.AssignmentId == assignmentId)
            .Select(a => a.Date.Date)
            .MinAsync(cancellationToken);

        var maxDate = await _context.Appointments
            .Where(a => a.AssignmentId == assignmentId)
            .Select(a => a.Date.Date)
            .MaxAsync(cancellationToken);

        List<AppointmentRow> result = new();

        for (var date = minDate; date <= maxDate; date = date.AddDays(1))
        {
            var dateCopy = date;
            var appointmentModels = await _context.Appointments
                .Where(a => a.AssignmentId == assignmentId
                            && a.Date.Date == dateCopy)
                .Select(a => new AppointmentModel
                {
                    AppointmentId = a.AppointmentId,
                    Time = TimeHelper.GetTime(a.TimeInMinutes),
                    TeacherName = a.Teacher.FullName,
                    TeacherEmail = a.Teacher.Email!,
                    IsAvailable = a.StudentId == null,
                    IsMine = a.StudentId == currentUserId
                })
                .ToListAsync(cancellationToken);

            result.Add(new AppointmentRow
            {
                Date = date,
                AppointmentModels = appointmentModels
            });
        }

        return result;
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

    public async Task<Result<int>> SignUpStudentAsync(int appointmentId, Guid studentId, CancellationToken cancellationToken = default)
    {
        var oldAppointment = await _context.Appointments
            .Where(a => a.StudentId == studentId)
            .SingleOrDefaultAsync(cancellationToken);

        var appointment = await _context.Appointments
            .Where(a => a.AppointmentId == appointmentId)
            .SingleOrDefaultAsync(cancellationToken);

        if (appointment is null)
        {
            return new NotFoundError(AppointmentErrorMessages.APPOINTMENT_NOT_FOUND);
        }

        if (appointment.StudentId is not null)
        {
            return new ConflictError(AppointmentErrorMessages.APPOINTMENT_SIGN_UP_CONFLICT);
        }

        if (oldAppointment is not null)
        {
            oldAppointment.StudentId = null;
        }

        appointment.StudentId = studentId;
        await _context.SaveChangesAsync(cancellationToken);

        return appointment.AssignmentId;
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