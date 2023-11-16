using FluentResults;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.BusinessLogic.Services.Authentication.Interfaces;
using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.Constants.Errors.Group;
using HomeworkManager.Model.CustomEntities.Assignment;
using HomeworkManager.Model.CustomEntities.Errors;

namespace HomeworkManager.BusinessLogic.Managers;

public class AssignmentManager : IAssignmentManager
{
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IGroupRepository _groupRepository;

    public AssignmentManager(IAssignmentRepository assignmentRepository, ICurrentUserService currentUserService, IGroupRepository groupRepository)
    {
        _assignmentRepository = assignmentRepository;
        _currentUserService = currentUserService;
        _groupRepository = groupRepository;
    }

    public async Task<Result<int>> CreateAsync(NewAssignment newAssignment, CancellationToken cancellationToken = default)
    {
        var groupId = await _groupRepository.GetIdByNameAsync(newAssignment.Group, cancellationToken);

        if (groupId is null)
        {
            return new BusinessError(GroupErrorMessages.GROUP_NOT_FOUND);
        }

        var userId = await _currentUserService.GetIdAsync(cancellationToken);

        return await _assignmentRepository.CreateAsync(newAssignment, (int)groupId, userId, cancellationToken);
    }

    public async Task<bool> NameAvailableAsync(NewAssignment newAssignment, CancellationToken cancellationToken = default)
    {
        var groupId = await _groupRepository.GetIdByNameAsync(newAssignment.Group, cancellationToken);

        if (groupId is null)
        {
            return false;
        }

        return !await _assignmentRepository.ExistsWithNameAsync((int)groupId, newAssignment.Name, cancellationToken);
    }
}