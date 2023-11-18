using FluentResults;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.BusinessLogic.Services.Authentication.Interfaces;
using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.Constants;
using HomeworkManager.Model.Constants.Errors.Assignment;
using HomeworkManager.Model.Constants.Errors.Group;
using HomeworkManager.Model.CustomEntities.Assignment;
using HomeworkManager.Model.CustomEntities.Errors;
using HomeworkManager.Model.Entities;

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

    public async Task<bool> ExistsWithIdAsync(int assignmentId, CancellationToken cancellationToken = default)
    {
        return await _assignmentRepository.ExistsWithIdAsync(assignmentId, cancellationToken);
    }

    public async Task<bool> ExistsDraftWithIdAsync(int assignmentId, CancellationToken cancellationToken = default)
    {
        return await _assignmentRepository.ExistsDraftWithIdAsync(assignmentId, cancellationToken);
    }

    public async Task<Result<AssignmentModel>> GetModelAsync(int assignmentId, CancellationToken cancellationToken = default)
    {
        var assignmentModel = await _assignmentRepository.GetModelByIdAsync(assignmentId, cancellationToken);

        if (assignmentModel is null)
        {
            return new NotFoundError(AssignmentErrorMessages.ASSIGNMENT_WITH_ID_NOT_FOUND);
        }

        return assignmentModel;
    }

    public async Task<Result<int>> CreateAsync(NewAssignment newAssignment, CancellationToken cancellationToken = default)
    {
        var groupId = await _groupRepository.GetIdByNameAsync(newAssignment.GroupInfo, cancellationToken);

        if (!groupId.HasValue)
        {
            return new BusinessError(GroupErrorMessages.GROUP_NOT_FOUND);
        }

        var userId = await _currentUserService.GetIdAsync(cancellationToken);

        return await _assignmentRepository.CreateAsync(newAssignment, groupId.Value, userId, cancellationToken);
    }

    public async Task<Result> UpdateAsync(int assignmentId, UpdatedAssignment updatedAssignment, CancellationToken cancellationToken = default)
    {
        if (await _currentUserService.HasRoleAsync(Roles.ADMINISTRATOR, cancellationToken))
        {
            return await _assignmentRepository.UpdateAsync(assignmentId, updatedAssignment, cancellationToken);
        }

        var userId = await _currentUserService.GetIdAsync(cancellationToken);

        return await _assignmentRepository.UpdateAsync(assignmentId, updatedAssignment, userId, cancellationToken);
    }

    public async Task<Result> PublishAsync(int assignmentId, CancellationToken cancellationToken = default)
    {
        var userId = await _currentUserService.GetIdAsync(cancellationToken);

        return await _assignmentRepository.PublishAsync(assignmentId, userId, cancellationToken);
    }

    public async Task<bool> NameAvailableAsync(NewAssignment newAssignment, CancellationToken cancellationToken = default)
    {
        var groupId = await _groupRepository.GetIdByNameAsync(newAssignment.GroupInfo, cancellationToken);

        if (!groupId.HasValue)
        {
            return false;
        }

        return !await _assignmentRepository.ExistsWithNameAsync(groupId.Value, newAssignment.Name, cancellationToken);
    }

    public async Task<bool> NameAvailableAsync(int assignmentId, string name, CancellationToken cancellationToken = default)
    {
        var oldName = await _assignmentRepository.GetNameByIdAsync(assignmentId, cancellationToken);

        if (oldName == name)
        {
            return true;
        }

        var groupId = await _assignmentRepository.GetGroupIdByIdAsync(assignmentId, cancellationToken);

        if (!groupId.HasValue)
        {
            return false;
        }

        return !await _assignmentRepository.ExistsWithNameAsync(groupId.Value, name, cancellationToken);
    }

    public async Task<bool> IsInGroupAsync(int assignmentId, CancellationToken cancellationToken = default)
    {
        var groupId = await _assignmentRepository.GetGroupIdByIdAsync(assignmentId, cancellationToken);

        if (!groupId.HasValue)
        {
            return false;
        }

        var userId = await _currentUserService.GetIdAsync(cancellationToken);

        return await _groupRepository.IsInGroupAsync(groupId.Value, userId, cancellationToken);
    }

    public async Task<bool> IsTeacherAsync(int assignmentId, CancellationToken cancellationToken = default)
    {
        var groupId = await _assignmentRepository.GetGroupIdByIdAsync(assignmentId, cancellationToken);

        if (!groupId.HasValue)
        {
            return false;
        }

        var userId = await _currentUserService.GetIdAsync(cancellationToken);

        return await _groupRepository.IsTeacherAsync(groupId.Value, userId, cancellationToken);
    }

    public async Task<bool> IsCreatorAsync(int assignmentId, CancellationToken cancellationToken = default)
    {
        var userId = await _currentUserService.GetIdAsync(cancellationToken);

        return await _assignmentRepository.IsCreatorAsync(assignmentId, userId, cancellationToken);
    }

    public async Task<IEnumerable<AssignmentType>> GetAssignmentTypes(CancellationToken cancellationToken = default)
    {
        return await _assignmentRepository.GetAssignmentTypes(cancellationToken);
    }
}