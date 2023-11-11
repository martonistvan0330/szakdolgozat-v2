using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.Constants;
using HomeworkManager.Model.Constants.Errors.Authentication;
using HomeworkManager.Model.Constants.Errors.Group;
using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.Course;
using HomeworkManager.Model.CustomEntities.Group;
using HomeworkManager.Model.CustomEntities.User;
using HomeworkManager.Model.ErrorEntities;

namespace HomeworkManager.BusinessLogic.Managers;

public class CourseManager : ICourseManager
{
    private const string GENERAL_GROUP_NAME = "General";

    private readonly ICourseRepository _courseRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly UserManager _userManager;
    private readonly IUserRepository _userRepository;

    public CourseManager(
        ICourseRepository courseRepository,
        IGroupRepository groupRepository,
        UserManager userManager,
        IUserRepository userRepository
    )
    {
        _courseRepository = courseRepository;
        _groupRepository = groupRepository;
        _userManager = userManager;
        _userRepository = userRepository;
    }

    public async Task<Result<IEnumerable<CourseCard>, BusinessError>> GetAllCoursesByUserAsync(string? username)
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

        if (await _userManager.IsInRoleAsync(user, Roles.ADMINISTRATOR))
        {
            return Result<IEnumerable<CourseCard>, BusinessError>.Ok((await _courseRepository.GetAllAsync()).OrderBy(c => c.Name));
        }

        return Result<IEnumerable<CourseCard>, BusinessError>.Ok((await _courseRepository.GetAllByUserAsync(user.Id)).OrderBy(c => c.Name));
    }

    public async Task<bool> ExistsAsync(int courseId)
    {
        return await _courseRepository.ExistsAsync(courseId);
    }

    public async Task<Result<CourseModel?, BusinessError>> GetModelByUserAsync(int courseId, string? username)
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

        if (await _userManager.IsInRoleAsync(user, Roles.ADMINISTRATOR))
        {
            return Result<CourseModel?, BusinessError>.Ok(await _courseRepository.GetModelAsync(courseId));
        }

        return Result<CourseModel?, BusinessError>.Ok(await _courseRepository.GetModelByUserAsync(courseId, user.Id));
    }

    public async Task<Result<IEnumerable<UserListRow>, BusinessError>> GetTeachersAsync(int courseId, string? username)
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
            && !await _courseRepository.IsInCourseAsync(courseId, user.Id))
        {
            return new BusinessError(GroupErrorMessages.NOT_IN_GROUP);
        }

        return Result<IEnumerable<UserListRow>, BusinessError>.Ok(await _courseRepository.GetTeachersAsync(courseId));
    }

    public async Task<Result<IEnumerable<UserListRow>, BusinessError>> GetStudentsAsync(int courseId, string? username)
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
            && !await _courseRepository.IsInCourseAsync(courseId, user.Id))
        {
            return new BusinessError(GroupErrorMessages.NOT_IN_GROUP);
        }

        return Result<IEnumerable<UserListRow>, BusinessError>.Ok(await _courseRepository.GetStudentsAsync(courseId));
    }

    public async Task<Result<IEnumerable<UserListRow>, BusinessError>> GetAddableTeachersAsync(int courseId, string? username)
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

        if (!await _userManager.IsInRoleAsync(user, Roles.ADMINISTRATOR))
        {
            return new BusinessError(AuthenticationErrorMessages.FORBIDDEN);
        }

        var users = await _userRepository.GetAllModelsAsync();
        var courseTeachers = await _courseRepository.GetTeachersAsync(courseId);
        var courseStudents = await _courseRepository.GetStudentsAsync(courseId);

        var addableTeachers = users
            .ExceptBy(
                courseTeachers.Select(u => u.UserId),
                u => u.UserId
            ).ExceptBy(
                courseStudents.Select(u => u.UserId),
                u => u.UserId
            );

        return Result<IEnumerable<UserListRow>, BusinessError>.Ok(addableTeachers);
    }

    public async Task<Result<IEnumerable<UserListRow>, BusinessError>> GetAddableStudentsAsync(int courseId, string? username)
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

        if (!await _userManager.IsInRoleAsync(user, Roles.ADMINISTRATOR))
        {
            return new BusinessError(AuthenticationErrorMessages.FORBIDDEN);
        }

        var users = await _userRepository.GetAllModelsAsync();
        var courseStudents = await _courseRepository.GetStudentsAsync(courseId);
        var courseTeachers = await _courseRepository.GetTeachersAsync(courseId);

        var addableStudents = users
            .ExceptBy(
                courseStudents.Select(u => u.UserId),
                u => u.UserId
            ).ExceptBy(
                courseTeachers.Select(u => u.UserId),
                u => u.UserId
            );

        return Result<IEnumerable<UserListRow>, BusinessError>.Ok(addableStudents);
    }

    public async Task<Result<int, BusinessError>> CreateAsync(NewCourse newCourse, string? username)
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

        var courseCreateResult = await _courseRepository.CreateAsync(newCourse, user);

        if (!courseCreateResult.Success)
        {
            return courseCreateResult.Error!;
        }

        var newGroup = new NewGroup
        {
            Name = "General",
            Description = newCourse.Description
        };

        var groupCreateError = await _groupRepository.CreateAsync(newGroup, courseCreateResult.Value, user);

        if (!groupCreateError.Success)
        {
            return groupCreateError.Error!;
        }

        return courseCreateResult.Value;
    }

    public async Task<BusinessError?> UpdateAsync(int courseId, UpdateCourse updatedCourse, string? username)
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

        if (await _userManager.IsInRoleAsync(user, Roles.ADMINISTRATOR))
        {
            return await _courseRepository.UpdateAsync(courseId, updatedCourse);
        }

        return await _courseRepository.UpdateAsync(courseId, updatedCourse, user);
    }

    public async Task<BusinessError?> AddTeachersAsync(int courseId, string? username, ICollection<Guid> userIds)
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
            && !await _courseRepository.IsCreatorAsync(courseId, user.Id))
        {
            return new BusinessError(AuthenticationErrorMessages.FORBIDDEN);
        }

        await _courseRepository.AddTeachersAsync(courseId, userIds);
        await _groupRepository.AddTeachersAsync(courseId, GENERAL_GROUP_NAME, userIds);

        return null;
    }

    public async Task<BusinessError?> AddStudentsAsync(int courseId, string? username, ICollection<Guid> userIds)
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
            && !await _courseRepository.IsCreatorAsync(courseId, user.Id))
        {
            return new BusinessError(AuthenticationErrorMessages.FORBIDDEN);
        }

        await _courseRepository.AddStudentsAsync(courseId, userIds);
        await _groupRepository.AddStudentsAsync(courseId, GENERAL_GROUP_NAME, userIds);

        return null;
    }

    public async Task<bool> IsInCourseAsync(int courseId, string? username)
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

        return await _courseRepository.IsInCourseAsync(courseId, user.Id);
    }

    public async Task<bool> IsCreatorAsync(int courseId, string? username)
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

        return await _courseRepository.IsCreatorAsync(courseId, user.Id);
    }

    public async Task<bool> IsTeacherAsync(int courseId, string? username)
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

        return await _courseRepository.IsTeacherAsync(courseId, user.Id);
    }

    public async Task<bool> NameAvailableAsync(string name)
    {
        return !await _courseRepository.ExistsWithNameAsync(name);
    }
}