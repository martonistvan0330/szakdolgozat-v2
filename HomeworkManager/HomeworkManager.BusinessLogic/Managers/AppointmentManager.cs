using FluentResults;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.BusinessLogic.Services.Authentication.Interfaces;
using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.CustomEntities.Appointment;

namespace HomeworkManager.BusinessLogic.Managers;

public class AppointmentManager : IAppointmentManager
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly ICurrentUserService _currentUserService;

    public AppointmentManager(IAppointmentRepository appointmentRepository, ICurrentUserService currentUserService)
    {
        _appointmentRepository = appointmentRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Result> CreateAsync(NewAppointment newAppointment, CancellationToken cancellationToken = default)
    {
        var currentUserId = await _currentUserService.GetIdAsync(cancellationToken);

        var appointmentTimes = GetAppointments(newAppointment.StartTime, newAppointment.EndTime, newAppointment.PresentationLength);
        await _appointmentRepository.CreateAsync(newAppointment.AssignmentId, newAppointment.Date, appointmentTimes, currentUserId,
            cancellationToken);

        return Result.Ok();
    }
    
    public async Task<bool> AvailableAsync(NewAppointment newAppointment, CancellationToken cancellationToken = default)
    {
        int startMinutes = GetMinutes(newAppointment.StartTime);
        int endMinutes = GetMinutes(newAppointment.EndTime);
        
        var currentUserId = await _currentUserService.GetIdAsync(cancellationToken);

        return !await _appointmentRepository.ExistsBetweenAsync(
            newAppointment.AssignmentId,
            newAppointment.Date,
            startMinutes,
            endMinutes,
            currentUserId,
            cancellationToken
        );
    }

    private IEnumerable<int> GetAppointments(string startTime, string endTime, int length)
    {
        int startMinutes = GetMinutes(startTime);
        int endMinutes = GetMinutes(endTime);

        List<int> appointmentTimes = new();

        for (int minutes = startMinutes; minutes < endMinutes - length; minutes += length)
        {
            appointmentTimes.Add(minutes);
        }

        return appointmentTimes;
    }

    private int GetMinutes(string time)
    {
        string[] parts = time.Split(':');
        int hours = int.Parse(parts[0]);
        int minutes = int.Parse(parts[1]);

        return hours * 60 + minutes;
    }

    private string GetTime(int mins)
    {
        int hours = mins / 60;
        int minutes = mins % 60;
        return $"{hours}:{minutes}";
    }
}