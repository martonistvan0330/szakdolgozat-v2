using System.Transactions;
using FluentResults;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.BusinessLogic.Services.Authentication.Interfaces;
using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.Constants;
using HomeworkManager.Model.Constants.Errors;
using HomeworkManager.Model.Constants.Errors.Course;
using HomeworkManager.Model.CustomEntities.Course;
using HomeworkManager.Model.CustomEntities.Errors;
using HomeworkManager.Model.CustomEntities.Group;
using HomeworkManager.Model.CustomEntities.User;

namespace HomeworkManager.BusinessLogic.Managers;

public class CourseManager : ICourseManager
{
    private readonly ICourseRepository _courseRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IGroupRepository _groupRepository;
    private readonly IUserManager _userManager;
    private readonly IUserRepository _userRepository;

    public CourseManager(
        ICourseRepository courseRepository,
        ICurrentUserService currentUserService,
        IGroupRepository groupRepository,
        IUserManager userManager,
        IUserRepository userRepository
    )
    {
        _courseRepository = courseRepository;
        _currentUserService = currentUserService;
        _groupRepository = groupRepository;
        _userManager = userManager;
        _userRepository = userRepository;
    }

    public async Task<bool> ExistsWithIdAsync(int courseId, CancellationToken cancellationToken = default)
    {
        return await _courseRepository.ExistsWithIdAsync(courseId, cancellationToken);
    }

    public async Task<bool> NameAvailableAsync(UpdatedCourse updatedCourse, CancellationToken cancellationToken = default)
    {
        if (!updatedCourse.CourseId.HasValue)
        {
            return false;
        }

        var courseName = await _courseRepository.GetNameByIdAsync(updatedCourse.CourseId.Value, cancellationToken);

        if (courseName is not null && courseName == updatedCourse.Name)
        {
            return true;
        }

        return !await _courseRepository.ExistsWithNameAsync(updatedCourse.Name, cancellationToken);
    }

    public async Task<bool> NameAvailableAsync(string name, CancellationToken cancellationToken = default)
    {
        return !await _courseRepository.ExistsWithNameAsync(name, cancellationToken);
    }

    public async Task<Result<CourseModel>> GetModelAsync(int courseId, CancellationToken cancellationToken = default)
    {
        CourseModel? courseModel;

        if (await _currentUserService.HasRoleAsync(Roles.ADMINISTRATOR, cancellationToken))
        {
            courseModel = await _courseRepository.GetModelAsync(courseId, cancellationToken);
        }
        else
        {
            var userId = await _currentUserService.GetIdAsync(cancellationToken);

            courseModel = await _courseRepository.GetModelAsync(courseId, userId, cancellationToken);
        }

        if (courseModel is null)
        {
            return new NotFoundError(CourseErrorMessages.COURSE_NOT_FOUND);
        }

        return courseModel;
    }

    public async Task<Result<IEnumerable<CourseCard>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        IEnumerable<CourseCard> courseCards;

        if (await _currentUserService.HasRoleAsync(Roles.ADMINISTRATOR, cancellationToken))
        {
            courseCards = await _courseRepository.GetAllAsync(cancellationToken);
        }

        else
        {
            var userId = await _currentUserService.GetIdAsync(cancellationToken);

            courseCards = await _courseRepository.GetAllAsync(userId, cancellationToken);
        }

        return courseCards.OrderBy(c => c.Name).ToList();
    }

    public async Task<Result<IEnumerable<UserListRow>>> GetTeachersAsync(int courseId, CancellationToken cancellationToken = default)
    {
        return Result.Ok(await _courseRepository.GetTeachersAsync(courseId, cancellationToken));
    }

    public async Task<Result<IEnumerable<UserListRow>>> GetStudentsAsync(int courseId, CancellationToken cancellationToken = default)
    {
        return Result.Ok(await _courseRepository.GetStudentsAsync(courseId, cancellationToken));
    }

    public async Task<Result<IEnumerable<UserListRow>>> GetAddableTeachersAsync(int courseId, CancellationToken cancellationToken = default)
    {
        var teachers = await _userRepository.GetAllModelByRoleAsync(Roles.TEACHER, cancellationToken);
        var courseTeachers = await _courseRepository.GetTeachersAsync(courseId, cancellationToken);
        var courseStudents = await _courseRepository.GetStudentsAsync(courseId, cancellationToken);

        var addableTeachers = teachers
            .ExceptBy(
                courseTeachers.Select(u => u.UserId),
                u => u.UserId
            )
            .ExceptBy
            (
                courseStudents.Select(u => u.UserId),
                u => u.UserId
            );

        return Result.Ok(addableTeachers);
    }

    public async Task<Result<IEnumerable<UserListRow>>> GetAddableStudentsAsync(int courseId, CancellationToken cancellationToken = default)
    {
        var students = await _userRepository.GetAllModelByRoleAsync(Roles.STUDENT, cancellationToken);
        var courseStudents = await _courseRepository.GetStudentsAsync(courseId, cancellationToken);
        var courseTeachers = await _courseRepository.GetTeachersAsync(courseId, cancellationToken);

        var addableStudents = students
            .ExceptBy(
                courseStudents.Select(u => u.UserId),
                u => u.UserId
            ).ExceptBy(
                courseTeachers.Select(u => u.UserId),
                u => u.UserId
            );

        return Result.Ok(addableStudents);
    }

    public async Task<Result<int>> CreateAsync(NewCourse newCourse, CancellationToken cancellationToken = default)
    {
        var user = await _currentUserService.GetAsync(cancellationToken);

        using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        var courseId = await _courseRepository.CreateAsync(newCourse, user, cancellationToken);

        var newGroup = new NewGroup
        {
            Name = Constants.GENERAL_GROUP_NAME,
            Description = newCourse.Description
        };

        await _groupRepository.CreateAsync(newGroup, courseId, user, cancellationToken);

        transactionScope.Complete();

        return courseId;
    }

    public async Task<Result> UpdateAsync(int courseId, UpdatedCourse updatedCourse, CancellationToken cancellationToken = default)
    {
        if (await _currentUserService.HasRoleAsync(Roles.ADMINISTRATOR, cancellationToken))
        {
            return await _courseRepository.UpdateAsync(courseId, updatedCourse, cancellationToken);
        }

        var userId = await _currentUserService.GetIdAsync(cancellationToken);

        return await _courseRepository.UpdateAsync(courseId, updatedCourse, userId, cancellationToken);
    }

    public async Task<Result> AddTeachersAsync(int courseId, ICollection<Guid> userIds, CancellationToken cancellationToken = default)
    {
        using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        var courseAddResult = await _courseRepository.AddTeachersAsync(courseId, userIds, cancellationToken);

        if (!courseAddResult.IsSuccess)
        {
            return courseAddResult;
        }

        var groupAddResult = await _groupRepository.AddTeachersAsync(courseId, Constants.GENERAL_GROUP_NAME, userIds, cancellationToken);

        if (!groupAddResult.IsSuccess)
        {
            return groupAddResult;
        }

        transactionScope.Complete();

        return Result.Ok();
    }

    public async Task<Result> AddStudentsAsync(int courseId, ICollection<Guid> userIds, CancellationToken cancellationToken = default)
    {
        using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        var courseAddResult = await _courseRepository.AddStudentsAsync(courseId, userIds, cancellationToken);

        if (!courseAddResult.IsSuccess)
        {
            return courseAddResult;
        }

        var groupAddResult = await _groupRepository.AddStudentsAsync(courseId, Constants.GENERAL_GROUP_NAME, userIds, cancellationToken);

        if (!groupAddResult.IsSuccess)
        {
            return groupAddResult;
        }

        transactionScope.Complete();

        return Result.Ok();
    }

    public async Task<bool> IsInCourseAsync(int courseId, CancellationToken cancellationToken = default)
    {
        var userId = await _currentUserService.GetIdAsync(cancellationToken);

        return await _courseRepository.IsInCourseAsync(courseId, userId, cancellationToken);
    }

    public async Task<bool> IsCreatorAsync(int courseId, CancellationToken cancellationToken = default)
    {
        var userId = await _currentUserService.GetIdAsync(cancellationToken);

        return await _courseRepository.IsCreatorAsync(courseId, userId, cancellationToken);
    }

    public async Task<bool> IsTeacherAsync(int courseId, CancellationToken cancellationToken = default)
    {
        var userId = await _currentUserService.GetIdAsync(cancellationToken);

        return await _courseRepository.IsTeacherAsync(courseId, userId, cancellationToken);
    }
}