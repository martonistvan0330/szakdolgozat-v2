using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.Constants;
using HomeworkManager.Model.Constants.Errors.Authentication;
using HomeworkManager.Model.Constants.Errors.Group;
using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.Group;
using HomeworkManager.Model.CustomEntities.User;
using HomeworkManager.Model.Entities;
using HomeworkManager.Model.ErrorEntities;
using Microsoft.AspNetCore.Identity;

namespace HomeworkManager.BusinessLogic.Managers;

public class GroupManager : IGroupManager
{
    private const string GENERAL_GROUP_NAME = "General";

    private readonly ICourseRepository _courseRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IUserManager _userManager;

    public GroupManager(ICourseRepository courseRepository, IGroupRepository groupRepository, IUserManager userManager)
    {
        _courseRepository = courseRepository;
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
        return await _groupRepository.ExistsWithNameAsync(courseId, groupName);
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

    public async Task<Result<Pageable<UserListRow>, BusinessError>> GetTeachersAsync
    (
        int courseId,
        string groupName,
        string? username,
        PageableOptions options
    )
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

        if (!await _userManager.IsInRoleAsync(user, Roles.ADMINISTRATOR)
            && !await _groupRepository.IsInGroupAsync(courseId, groupName, user.Id))
        {
            return new BusinessError(GroupErrorMessages.NOT_IN_GROUP);
        }

        var teacherCount = await _groupRepository.GetTeacherCountAsync(courseId, groupName, options.SearchText);

        var teachers = options.SortOptions?.Sort switch
        {
            "userId" => await _groupRepository.GetTeachersAsync(
                courseId, groupName,
                options.PageData,
                u => u.UserId,
                options.SortOptions.SortDirection.ToSortDirection(),
                options.SearchText),
            "fullName" => await _groupRepository.GetTeachersAsync(
                courseId, groupName,
                options.PageData,
                u => u.FullName,
                options.SortOptions.SortDirection.ToSortDirection(),
                options.SearchText),
            "username" => await _groupRepository.GetTeachersAsync(
                courseId, groupName,
                options.PageData,
                u => u.Username,
                options.SortOptions.SortDirection.ToSortDirection(),
                options.SearchText),
            "email" => await _groupRepository.GetTeachersAsync(
                courseId, groupName,
                options.PageData,
                u => u.Email,
                options.SortOptions.SortDirection.ToSortDirection(),
                options.SearchText),
            _ => await _groupRepository.GetTeachersAsync(courseId, groupName, options.PageData)
        };

        return Result<Pageable<UserListRow>, BusinessError>.Ok(new Pageable<UserListRow>
        {
            Items = teachers,
            TotalCount = teacherCount
        });
    }

    public async Task<Result<Pageable<UserListRow>, BusinessError>> GetStudentsAsync
    (
        int courseId,
        string groupName,
        string? username,
        PageableOptions options
    )
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

        if (!await _userManager.IsInRoleAsync(user, Roles.ADMINISTRATOR)
            && !await _groupRepository.IsInGroupAsync(courseId, groupName, user.Id))
        {
            return new BusinessError(GroupErrorMessages.NOT_IN_GROUP);
        }

        var studentCount = await _groupRepository.GetStudentCountAsync(courseId, groupName, options.SearchText);

        var students = options.SortOptions?.Sort switch
        {
            "userId" => await _groupRepository.GetStudentsAsync(
                courseId, groupName,
                options.PageData,
                u => u.UserId,
                options.SortOptions.SortDirection.ToSortDirection(),
                options.SearchText),
            "fullName" => await _groupRepository.GetStudentsAsync(
                courseId, groupName,
                options.PageData,
                u => u.FullName,
                options.SortOptions.SortDirection.ToSortDirection(),
                options.SearchText),
            "username" => await _groupRepository.GetStudentsAsync(
                courseId, groupName,
                options.PageData,
                u => u.Username,
                options.SortOptions.SortDirection.ToSortDirection(),
                options.SearchText),
            "email" => await _groupRepository.GetStudentsAsync(
                courseId, groupName,
                options.PageData,
                u => u.Email,
                options.SortOptions.SortDirection.ToSortDirection(),
                options.SearchText),
            _ => await _groupRepository.GetStudentsAsync(courseId, groupName, options.PageData)
        };

        return Result<Pageable<UserListRow>, BusinessError>.Ok(new Pageable<UserListRow>
        {
            Items = students,
            TotalCount = studentCount
        });
    }

    public async Task<Result<IEnumerable<UserListRow>, BusinessError>> GetAddableTeachersAsync(int courseId, string groupName, string? username)
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

        if (!await _userManager.IsInRoleAsync(user, Roles.ADMINISTRATOR)
            && !await _groupRepository.IsInGroupAsync(courseId, groupName, user.Id))
        {
            return new BusinessError(GroupErrorMessages.NOT_IN_GROUP);
        }

        var courseTeachers = await _courseRepository.GetTeachersAsync(courseId);
        var groupTeachers = await _groupRepository.GetTeachersAsync(courseId, groupName);
        var addableTeachers = courseTeachers.ExceptBy(
            groupTeachers.Select(u => u.UserId),
            u => u.UserId
        );

        return Result<IEnumerable<UserListRow>, BusinessError>.Ok(addableTeachers);
    }

    public async Task<Result<IEnumerable<UserListRow>, BusinessError>> GetAddableStudentsAsync(int courseId, string groupName, string? username)
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

        if (!await _userManager.IsInRoleAsync(user, Roles.ADMINISTRATOR)
            && !await _groupRepository.IsInGroupAsync(courseId, groupName, user.Id))
        {
            return new BusinessError(GroupErrorMessages.NOT_IN_GROUP);
        }

        var courseStudents = await _courseRepository.GetStudentsAsync(courseId);
        var groupStudents = await _groupRepository.GetStudentsAsync(courseId, groupName);
        var addableStudents = courseStudents.ExceptBy(
            groupStudents.Select(u => u.UserId),
            u => u.UserId
        );

        return Result<IEnumerable<UserListRow>, BusinessError>.Ok(addableStudents);
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

    public async Task<BusinessError?> AddTeachersAsync(int courseId, string groupName, string? username, IEnumerable<Guid> userIds)
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

        if (!await _userManager.IsInRoleAsync(user, Roles.ADMINISTRATOR)
            && !await _groupRepository.IsCreatorAsync(courseId, groupName, user.Id))
        {
            return new BusinessError(AuthenticationErrorMessages.FORBIDDEN);
        }

        await _groupRepository.AddTeachersAsync(courseId, groupName, userIds);

        return null;
    }

    public async Task<BusinessError?> AddStudentsAsync(int courseId, string groupName, string? username, IEnumerable<Guid> userIds)
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

        if (!await _userManager.IsInRoleAsync(user, Roles.ADMINISTRATOR)
            && !await _groupRepository.IsCreatorAsync(courseId, groupName, user.Id))
        {
            return new BusinessError(AuthenticationErrorMessages.FORBIDDEN);
        }

        await _groupRepository.AddStudentsAsync(courseId, groupName, userIds);

        return null;
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

        return await _groupRepository.IsInGroupAsync(courseId, groupName, user.Id);
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

        return await _groupRepository.IsCreatorAsync(courseId, groupName, user.Id);
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

        return await _groupRepository.IsTeacherAsync(courseId, groupName, user.Id);
    }

    public async Task<bool> NameAvailableAsync(string name, int courseId)
    {
        return !await _groupRepository.ExistsWithNameAsync(courseId, name);
    }
}