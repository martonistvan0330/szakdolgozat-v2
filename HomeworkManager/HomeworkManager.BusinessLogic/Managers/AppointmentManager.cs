using System.Transactions;
using FluentResults;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.BusinessLogic.Services.Authentication.Interfaces;
using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.CustomEntities.Appointment;
using HomeworkManager.Shared.Services;

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

    public async Task<Result<IEnumerable<AppointmentRow>>> GetAllByAssignmentAsync(int assignmentId, CancellationToken cancellationToken = default)
    {
        var currentUserId = await _currentUserService.GetIdAsync(cancellationToken);

        return Result.Ok(await _appointmentRepository.GetAllByAssignmentIdAsync(assignmentId, currentUserId, cancellationToken));
    }

    public async Task<Result> CreateAsync(NewAppointment newAppointment, CancellationToken cancellationToken = default)
    {
        var currentUserId = await _currentUserService.GetIdAsync(cancellationToken);

        var appointmentTimes = GetAppointments(newAppointment.StartTime, newAppointment.EndTime, newAppointment.PresentationLength);
        await _appointmentRepository.CreateAsync(newAppointment.AssignmentId, newAppointment.Date, appointmentTimes, currentUserId,
            cancellationToken);

        return Result.Ok();
    }

    public async Task<Result<int>> SignUpAsync(int appointmentId, CancellationToken cancellationToken = default)
    {
        using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        var currentUserId = await _currentUserService.GetIdAsync(cancellationToken);

        var signUpResult = await _appointmentRepository.SignUpStudentAsync(appointmentId, currentUserId, cancellationToken);

        if (signUpResult.IsSuccess)
        {
            transactionScope.Complete();
        }

        return signUpResult;
    }

    public async Task<bool> AvailableAsync(NewAppointment newAppointment, CancellationToken cancellationToken = default)
    {
        int startMinutes = TimeHelper.GetMinutes(newAppointment.StartTime);
        int endMinutes = TimeHelper.GetMinutes(newAppointment.EndTime);

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
        int startMinutes = TimeHelper.GetMinutes(startTime);
        int endMinutes = TimeHelper.GetMinutes(endTime);

        List<int> appointmentTimes = new();

        for (int minutes = startMinutes; minutes < endMinutes - length; minutes += length)
        {
            appointmentTimes.Add(minutes);
        }

        return appointmentTimes;
    }
}