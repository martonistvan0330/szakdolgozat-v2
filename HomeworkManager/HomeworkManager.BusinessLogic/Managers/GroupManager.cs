using FluentResults;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.BusinessLogic.Services.Authentication.Interfaces;
using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.Constants;
using HomeworkManager.Model.Constants.Errors;
using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.Group;
using HomeworkManager.Model.CustomEntities.User;

namespace HomeworkManager.BusinessLogic.Managers;

public class GroupManager : IGroupManager
{
    private readonly ICourseRepository _courseRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IGroupRepository _groupRepository;
    private readonly IUserManager _userManager;

    public GroupManager
    (
        ICourseRepository courseRepository,
        ICurrentUserService currentUserService,
        IGroupRepository groupRepository,
        IUserManager userManager
    )
    {
        _courseRepository = courseRepository;
        _currentUserService = currentUserService;
        _groupRepository = groupRepository;
        _userManager = userManager;
    }

    public async Task<bool> ExistsWithNameAsync(int courseId, string groupName, CancellationToken cancellationToken = default)
    {
        return await _groupRepository.ExistsWithNameAsync(courseId, groupName, cancellationToken);
    }
    
    public async Task<bool> NameAvailableAsync(int courseId, string name, CancellationToken cancellationToken = default)
    {
        return !await _groupRepository.ExistsWithNameAsync(courseId, name, cancellationToken);
    }
    
    public async Task<bool> NameAvailableAsync(int courseId, string groupName, string name, CancellationToken cancellationToken = default)
    {
        if (groupName == name)
        {
            return true;
        }
        
        return !await _groupRepository.ExistsWithNameAsync(courseId, name, cancellationToken);
    }
    
    public async Task<Result<IEnumerable<GroupListRow>>> GetAllAsync(int courseId, CancellationToken cancellationToken = default)
    {
        IEnumerable<GroupListRow> groups;

        if (await _currentUserService.HasRoleAsync(Roles.ADMINISTRATOR, cancellationToken))
        {
            groups = await _groupRepository.GetAllAsync(courseId, cancellationToken);
        }
        else
        {
            var userId = await _currentUserService.GetIdAsync(cancellationToken);
            
            groups = await _groupRepository.GetAllAsync(courseId, userId, cancellationToken);
        }

        var orderedGroups = groups
            .OrderBy(g => g.Name == Constants.GENERAL_GROUP_NAME ? 0 : 1)
            .ThenBy(g => g.Name)
            .ToList();
        
        return orderedGroups;
    }

    public async Task<Result<GroupModel?>> GetModelAsync(int courseId, string groupName, CancellationToken cancellationToken = default)
    {
        GroupModel? group;

        if (await _currentUserService.HasRoleAsync(Roles.ADMINISTRATOR, cancellationToken))
        {
            group = await _groupRepository.GetModelAsync(courseId, groupName, cancellationToken);
        }
        else
        {
            var userId = await _currentUserService.GetIdAsync(cancellationToken);
            
            group = await _groupRepository.GetModelAsync(courseId, groupName, userId, cancellationToken);
        }

        return group;
    }

    public async Task<Result<Pageable<UserListRow>>> GetTeachersAsync
    (
        int courseId,
        string groupName,
        PageableOptions options,
        CancellationToken cancellationToken = default
    )
    {
        var teacherCount = await _groupRepository.GetTeacherCountAsync(courseId, groupName, options.SearchText, cancellationToken);

        var teachers = options.SortOptions?.Sort switch
        {
            "userId" => await _groupRepository.GetTeachersAsync(
                courseId, groupName,
                options.PageData,
                u => u.UserId,
                options.SortOptions.SortDirection.ToSortDirection(),
                options.SearchText,
                cancellationToken),
            "fullName" => await _groupRepository.GetTeachersAsync(
                courseId, groupName,
                options.PageData,
                u => u.FullName,
                options.SortOptions.SortDirection.ToSortDirection(),
                options.SearchText,
                cancellationToken),
            "username" => await _groupRepository.GetTeachersAsync(
                courseId, groupName,
                options.PageData,
                u => u.Username,
                options.SortOptions.SortDirection.ToSortDirection(),
                options.SearchText,
                cancellationToken),
            "email" => await _groupRepository.GetTeachersAsync(
                courseId, groupName,
                options.PageData,
                u => u.Email,
                options.SortOptions.SortDirection.ToSortDirection(),
                options.SearchText,
                cancellationToken),
            _ => await _groupRepository.GetTeachersAsync(courseId, groupName, options.PageData, cancellationToken: cancellationToken)
        };

        return new Pageable<UserListRow>
        {
            Items = teachers,
            TotalCount = teacherCount
        };
    }

    public async Task<Result<Pageable<UserListRow>>> GetStudentsAsync
    (
        int courseId,
        string groupName,
        PageableOptions options,
        CancellationToken cancellationToken = default
    )
    {
        var studentCount = await _groupRepository.GetStudentCountAsync(courseId, groupName, options.SearchText, cancellationToken);

        var students = options.SortOptions?.Sort switch
        {
            "userId" => await _groupRepository.GetStudentsAsync(
                courseId, groupName,
                options.PageData,
                u => u.UserId,
                options.SortOptions.SortDirection.ToSortDirection(),
                options.SearchText,
                cancellationToken),
            "fullName" => await _groupRepository.GetStudentsAsync(
                courseId, groupName,
                options.PageData,
                u => u.FullName,
                options.SortOptions.SortDirection.ToSortDirection(),
                options.SearchText,
                cancellationToken),
            "username" => await _groupRepository.GetStudentsAsync(
                courseId, groupName,
                options.PageData,
                u => u.Username,
                options.SortOptions.SortDirection.ToSortDirection(),
                options.SearchText,
                cancellationToken),
            "email" => await _groupRepository.GetStudentsAsync(
                courseId, groupName,
                options.PageData,
                u => u.Email,
                options.SortOptions.SortDirection.ToSortDirection(),
                options.SearchText,
                cancellationToken),
            _ => await _groupRepository.GetStudentsAsync(courseId, groupName, options.PageData, cancellationToken)
        };

        return new Pageable<UserListRow>
        {
            Items = students,
            TotalCount = studentCount,
        };
    }

    public async Task<Result<IEnumerable<UserListRow>>> GetAddableTeachersAsync(int courseId, string groupName, CancellationToken cancellationToken = default)
    {
        var courseTeachers = await _courseRepository.GetTeachersAsync(courseId, cancellationToken);
        var groupTeachers = await _groupRepository.GetTeachersAsync(courseId, groupName, cancellationToken: cancellationToken);
        var addableTeachers = courseTeachers.ExceptBy(
            groupTeachers.Select(u => u.UserId),
            u => u.UserId
        );

        return Result.Ok(addableTeachers);
    }

    public async Task<Result<IEnumerable<UserListRow>>> GetAddableStudentsAsync(int courseId, string groupName, CancellationToken cancellationToken = default)
    {
        var courseStudents = await _courseRepository.GetStudentsAsync(courseId, cancellationToken);
        var groupStudents = await _groupRepository.GetStudentsAsync(courseId, groupName, cancellationToken: cancellationToken);
        var addableStudents = courseStudents.ExceptBy(
            groupStudents.Select(u => u.UserId),
            u => u.UserId
        );

        return Result.Ok(addableStudents);
    }

    public async Task<Result<int>> CreateAsync(NewGroup newGroup, int courseId, CancellationToken cancellationToken = default)
    {
        var user = await _currentUserService.GetAsync(cancellationToken);
        
        return await _groupRepository.CreateAsync(newGroup, courseId, user, cancellationToken);
    }

    public async Task<Result> UpdateAsync(int courseId, string groupName, UpdatedGroup updatedGroup, CancellationToken cancellationToken = default)
    {
        if (await _currentUserService.HasRoleAsync(Roles.ADMINISTRATOR, cancellationToken))
        {
            return await _groupRepository.UpdateAsync(courseId, groupName, updatedGroup, cancellationToken);
        }
        
        var userId = await _currentUserService.GetIdAsync(cancellationToken);

        return await _groupRepository.UpdateAsync(courseId, groupName, updatedGroup, userId, cancellationToken);
    }

    public async Task<Result> AddTeachersAsync(int courseId, string groupName, IEnumerable<Guid> userIds, CancellationToken cancellationToken = default)
    {
        return await _groupRepository.AddTeachersAsync(courseId, groupName, userIds, cancellationToken);
    }

    public async Task<Result> AddStudentsAsync(int courseId, string groupName, IEnumerable<Guid> userIds, CancellationToken cancellationToken = default)
    {
        return await _groupRepository.AddStudentsAsync(courseId, groupName, userIds, cancellationToken);
    }

    public async Task<bool> IsInGroupAsync(int courseId, string groupName, CancellationToken cancellationToken = default)
    {
        var userId = await _currentUserService.GetIdAsync(cancellationToken);
        
        return await _groupRepository.IsInGroupAsync(courseId, groupName, userId, cancellationToken);
    }

    public async Task<bool> IsCreatorAsync(int courseId, string groupName, CancellationToken cancellationToken = default)
    {
        var userId = await _currentUserService.GetIdAsync(cancellationToken);

        return await _groupRepository.IsCreatorAsync(courseId, groupName, userId, cancellationToken);
    }

    public async Task<bool> IsTeacherAsync(int courseId, string groupName, CancellationToken cancellationToken = default)
    {
        var userId = await _currentUserService.GetIdAsync(cancellationToken);

        return await _groupRepository.IsTeacherAsync(courseId, groupName, userId, cancellationToken);
    }
}