using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.Constants;
using HomeworkManager.Model.Constants.Errors.Authentication;
using HomeworkManager.Model.Constants.Errors.Group;
using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.Group;
using HomeworkManager.Model.ErrorEntities;

namespace HomeworkManager.BusinessLogic.Managers;

public class GroupManager : IGroupManager
{
    private const string GENERAL_GROUP_NAME = "General";

    private readonly IGroupRepository _groupRepository;
    private readonly UserManager _userManager;

    public GroupManager(IGroupRepository groupRepository, UserManager userManager)
    {
        _groupRepository = groupRepository;
        _userManager = userManager;
    }

    public async Task<Result<IEnumerable<GroupListRow>, BusinessError>> GetAllByUserAsync(int courseId, string? username)
    {
        if (username is null)
        {
            return new BusinessError(AuthenticationErrorMessages.INVALID_USERNAME);
        }

        var user = await _userManager.FindByNameAsync(username);

        if (user is null)
        {
            return new BusinessError(AuthenticationErrorMessages.USER_NOT_FOUND);
        }

        IEnumerable<GroupListRow> groups;

        if (await _userManager.IsInRoleAsync(user, Roles.ADMINISTRATOR))
        {
            groups = await _groupRepository.GetAllAsync(courseId);
        }
        else
        {
            groups = await _groupRepository.GetAllByUserAsync(courseId, user.Id);
        }

        var orderedGroups = groups.OrderBy(g => g.Name == GENERAL_GROUP_NAME ? 0 : 1).ThenBy(g => g.Name);

        return Result<IEnumerable<GroupListRow>, BusinessError>.Ok(orderedGroups);
    }

    public async Task<bool> ExistsAsync(string groupName, int courseId)
    {
        return await _groupRepository.ExistsWithNameAsync(groupName, courseId);
    }

    public async Task<Result<GroupModel?, BusinessError>> GetModelByUserAsync(int courseId, string groupName, string? username)
    {
        if (username is null)
        {
            return new BusinessError(AuthenticationErrorMessages.INVALID_USERNAME);
        }

        var user = await _userManager.FindByNameAsync(username);

        if (user is null)
        {
            return new BusinessError(AuthenticationErrorMessages.USER_NOT_FOUND);
        }

        GroupModel? group;

        if (await _userManager.IsInRoleAsync(user, Roles.ADMINISTRATOR))
        {
            group = await _groupRepository.GetModelAsync(courseId, groupName);
        }
        else
        {
            group = await _groupRepository.GetModelByUserAsync(courseId, groupName, user.Id);
        }

        return Result<GroupModel?, BusinessError>.Ok(group);
    }

    public async Task<Result<int, BusinessError>> CreateAsync(NewGroup newGroup, int courseId, string? username)
    {
        if (username is null)
        {
            return new BusinessError(AuthenticationErrorMessages.INVALID_USERNAME);
        }

        var user = await _userManager.FindByNameAsync(username);

        if (user is null)
        {
            return new BusinessError(AuthenticationErrorMessages.USER_NOT_FOUND);
        }

        return await _groupRepository.CreateAsync(newGroup, courseId, user);
    }

    public async Task<BusinessError?> UpdateAsync(int courseId, string groupName, UpdateGroup updatedGroup, string? username)
    {
        if (username is null)
        {
            return new BusinessError(AuthenticationErrorMessages.INVALID_USERNAME);
        }

        var user = await _userManager.FindByNameAsync(username);

        if (user is null)
        {
            return new BusinessError(AuthenticationErrorMessages.USER_NOT_FOUND);
        }

        if (groupName == GENERAL_GROUP_NAME && updatedGroup.Name != GENERAL_GROUP_NAME)
        {
            return new BusinessError(GroupErrorMessages.GENERAL_GROUP_NAME_CHANGED);
        }

        if (await _userManager.IsInRoleAsync(user, Roles.ADMINISTRATOR))
        {
            return await _groupRepository.UpdateAsync(courseId, groupName, updatedGroup);
        }

        return await _groupRepository.UpdateAsync(courseId, groupName, updatedGroup, user);
    }

    public async Task<bool> IsInGroupAsync(string groupName, int courseId, string? username)
    {
        if (username is null)
        {
            return false;
        }

        var user = await _userManager.FindByNameAsync(username);

        if (user is null)
        {
            return false;
        }

        return await _groupRepository.IsInGroupAsync(groupName, courseId, user.Id);
    }

    public async Task<bool> IsCreatorAsync(string groupName, int courseId, string? username)
    {
        if (username is null)
        {
            return false;
        }

        var user = await _userManager.FindByNameAsync(username);

        if (user is null)
        {
            return false;
        }

        return await _groupRepository.IsCreatorAsync(groupName, courseId, user.Id);
    }

    public async Task<bool> IsTeacherAsync(string groupName, int courseId, string? username)
    {
        if (username is null)
        {
            return false;
        }

        var user = await _userManager.FindByNameAsync(username);

        if (user is null)
        {
            return false;
        }

        return await _groupRepository.IsTeacherAsync(groupName, courseId, user.Id);
    }

    public async Task<bool> NameAvailableAsync(string name, int courseId)
    {
        return !await _groupRepository.ExistsWithNameAsync(name, courseId);
    }
}