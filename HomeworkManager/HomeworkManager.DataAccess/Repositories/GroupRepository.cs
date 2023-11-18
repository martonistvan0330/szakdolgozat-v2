using System.Linq.Expressions;
using FluentResults;
using HomeworkManager.DataAccess.Repositories.Extensions;
using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.Constants.Errors.Group;
using HomeworkManager.Model.Constants.Errors.User;
using HomeworkManager.Model.Contexts;
using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.Assignment;
using HomeworkManager.Model.CustomEntities.Errors;
using HomeworkManager.Model.CustomEntities.Group;
using HomeworkManager.Model.CustomEntities.User;
using HomeworkManager.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomeworkManager.DataAccess.Repositories;

public class GroupRepository : IGroupRepository
{
    private readonly HomeworkManagerContext _context;

    public GroupRepository(HomeworkManagerContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsWithNameAsync(int courseId, string groupName, CancellationToken cancellationToken = default)
    {
        return await _context.Groups
            .Where(g => g.CourseId == courseId && g.Name == groupName)
            .AnyAsync(cancellationToken);
    }

    public async Task<int?> GetIdByNameAsync(GroupInfo groupInfo, CancellationToken cancellationToken = default)
    {
        return await _context.Groups
            .Where(g => g.CourseId == groupInfo.CourseId && g.Name == groupInfo.Name)
            .Select(g => g.GroupId)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<GroupModel?> GetModelAsync(int courseId, string groupName, CancellationToken cancellationToken = default)
    {
        return await _context.Groups
            .Where(g => g.CourseId == courseId && g.Name == groupName)
            .Select(g => new GroupModel
            {
                Name = g.Name,
                Description = g.Description
            })
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<GroupModel?> GetModelAsync(int courseId, string groupName, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Groups
            .Where(g => g.CourseId == courseId && g.Name == groupName)
            .Where(g => g.Teachers.Select(u => u.Id).Contains(userId)
                        || g.Students.Select(u => u.Id).Contains(userId))
            .Select(g => new GroupModel
            {
                Name = g.Name,
                Description = g.Description
            })
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<GroupListRow>> GetAllAsync(int courseId, CancellationToken cancellationToken = default)
    {
        return await _context.Groups
            .Where(g => g.CourseId == courseId)
            .Select(g => new GroupListRow
            {
                GroupId = g.GroupId,
                Name = g.Name
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<GroupListRow>> GetAllAsync(int courseId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Groups
            .Where(g => g.CourseId == courseId)
            .Where(g => g.Teachers.Select(u => u.Id).Contains(userId)
                        || g.Students.Select(u => u.Id).Contains(userId))
            .Select(g => new GroupListRow
            {
                GroupId = g.GroupId,
                Name = g.Name
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetAssignmentCountAsync(int courseId, string groupName, Guid userId, string? searchText = null,
        CancellationToken cancellationToken = default)
    {
        return await _context.Groups
            .Where(g => g.CourseId == courseId && g.Name == groupName)
            .SelectMany(g => g.Assignments)
            .Where(a => !a.IsDraft || a.CreatorId == userId)
            .Search(searchText)
            .CountAsync(cancellationToken);
    }

    public async Task<int> GetTeacherCountAsync(int courseId, string groupName, string? searchText = null,
        CancellationToken cancellationToken = default)
    {
        return await _context.Groups
            .Where(g => g.CourseId == courseId && g.Name == groupName)
            .SelectMany(g => g.Teachers)
            .Search(searchText)
            .CountAsync(cancellationToken);
    }

    public async Task<int> GetStudentCountAsync(int courseId, string groupName, string? searchText = null,
        CancellationToken cancellationToken = default)
    {
        return await _context.Groups
            .Where(g => g.CourseId == courseId && g.Name == groupName)
            .SelectMany(g => g.Students)
            .Search(searchText)
            .CountAsync(cancellationToken);
    }

    public async Task<IEnumerable<AssignmentListRow>> GetAssignmentsAsync<TKey>
    (
        int courseId,
        string groupName,
        Guid userId,
        PageData? pageData = null,
        Expression<Func<AssignmentListRow, TKey>>? orderBy = null,
        SortDirection sortDirection = SortDirection.Ascending,
        string? searchText = null,
        CancellationToken cancellationToken = default
    )
    {
        return await _context.Groups
            .Where(g => g.CourseId == courseId && g.Name == groupName)
            .SelectMany(g => g.Assignments)
            .Where(a => !a.IsDraft || a.CreatorId == userId)
            .Search(searchText)
            .ToListModel()
            .OrderByWithDirection(orderBy, sortDirection)
            .GetPage(pageData)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<UserListRow>> GetTeachersAsync<TKey>
    (
        int courseId,
        string groupName,
        PageData? pageData = null,
        Expression<Func<UserListRow, TKey>>? orderBy = null,
        SortDirection sortDirection = SortDirection.Ascending,
        string? searchText = null,
        CancellationToken cancellationToken = default
    )
    {
        return await _context.Groups
            .Where(g => g.CourseId == courseId && g.Name == groupName)
            .SelectMany(g => g.Teachers)
            .Search(searchText)
            .Select(u => new UserListRow
            {
                UserId = u.Id,
                FullName = u.FullName,
                Username = "",
                Email = u.Email!,
                Roles = ""
            })
            .OrderByWithDirection(orderBy, sortDirection)
            .GetPage(pageData)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<UserListRow>> GetStudentsAsync<TKey>
    (
        int courseId,
        string groupName,
        PageData? pageData = null,
        Expression<Func<UserListRow, TKey>>? orderBy = null,
        SortDirection sortDirection = SortDirection.Ascending,
        string? searchText = null,
        CancellationToken cancellationToken = default
    )
    {
        return await _context.Groups
            .Where(g => g.CourseId == courseId && g.Name == groupName)
            .SelectMany(g => g.Students)
            .Search(searchText)
            .Select(u => new UserListRow
            {
                UserId = u.Id,
                FullName = u.FullName,
                Username = "",
                Email = u.Email!,
                Roles = ""
            })
            .OrderByWithDirection(orderBy, sortDirection)
            .GetPage(pageData)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CreateAsync(NewGroup newGroup, int courseId, User user, CancellationToken cancellationToken = default)
    {
        Group group = new()
        {
            Name = newGroup.Name,
            Description = newGroup.Description,
            CourseId = courseId,
            CreatorId = user.Id
        };

        _context.Groups.Add(group);
        group.Teachers.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        return group.GroupId;
    }

    public async Task<Result> UpdateAsync(int courseId, string groupName, UpdatedGroup updatedGroup, CancellationToken cancellationToken = default)
    {
        var group = await GetByNameAsync(courseId, groupName, cancellationToken);

        if (group is null)
        {
            return new BusinessError(GroupErrorMessages.GROUP_NOT_FOUND);
        }

        return await UpdateAsync(group, updatedGroup, cancellationToken);
    }

    public async Task<Result> UpdateAsync(int courseId, string groupName, UpdatedGroup updatedGroup, Guid userId,
        CancellationToken cancellationToken = default)
    {
        var group = await GetByNameAsync(courseId, groupName, cancellationToken);

        if (group is null)
        {
            return new BusinessError(GroupErrorMessages.GROUP_NOT_FOUND);
        }

        if (group.CreatorId != userId)
        {
            return new ForbiddenError();
        }

        return await UpdateAsync(group, updatedGroup, cancellationToken);
    }

    public async Task<Result> AddTeachersAsync(int courseId, string groupName, IEnumerable<Guid> userIds,
        CancellationToken cancellationToken = default)
    {
        var group = await _context.Groups
            .Where(g => g.CourseId == courseId && g.Name == groupName)
            .Include(g => g.Teachers)
            .SingleOrDefaultAsync(cancellationToken);

        if (group is null)
        {
            return new NotFoundError(GroupErrorMessages.GROUP_NOT_FOUND);
        }

        var teachers = await _context.Users
            .Where(u => userIds.Contains(u.Id))
            .Where(u => u.ManagedCourses.Select(c => c.CourseId).Contains(group.CourseId))
            .ToListAsync(cancellationToken);

        group.Teachers = group.Teachers.UnionBy(teachers, u => u.Id).ToHashSet();

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }

    public async Task<Result> AddStudentsAsync(int courseId, string groupName, IEnumerable<Guid> userIds,
        CancellationToken cancellationToken = default)
    {
        var group = await _context.Groups
            .Where(g => g.CourseId == courseId && g.Name == groupName)
            .Include(g => g.Students)
            .SingleOrDefaultAsync(cancellationToken);

        if (group is null)
        {
            return new NotFoundError(GroupErrorMessages.GROUP_NOT_FOUND);
        }

        var students = await _context.Users
            .Where(u => userIds.Contains(u.Id))
            .Where(u => u.AttendedCourses.Select(c => c.CourseId).Contains(group.CourseId))
            .ToListAsync(cancellationToken);

        group.Students = group.Students.UnionBy(students, u => u.Id).ToHashSet();

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }

    public async Task<Result> RemoveTeacherAsync(int courseId, string groupName, Guid teacherId, CancellationToken cancellationToken = default)
    {
        var group = await _context.Groups
            .Where(g => g.CourseId == courseId && g.Name == groupName)
            .Include(g => g.Teachers)
            .SingleOrDefaultAsync(cancellationToken);

        if (group is null)
        {
            return new NotFoundError(GroupErrorMessages.GROUP_NOT_FOUND);
        }

        var teacher = await _context.Users
            .Where(u => u.Id == teacherId)
            .SingleOrDefaultAsync(cancellationToken);

        if (teacher is null)
        {
            return new NotFoundError(UserErrorMessages.USER_WITH_ID_NOT_FOUND);
        }

        group.Teachers.Remove(teacher);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }

    public async Task<Result> RemoveStudentAsync(int courseId, string groupName, Guid studentId, CancellationToken cancellationToken = default)
    {
        var group = await _context.Groups
            .Where(g => g.CourseId == courseId && g.Name == groupName)
            .Include(g => g.Students)
            .SingleOrDefaultAsync(cancellationToken);

        if (group is null)
        {
            return new NotFoundError(GroupErrorMessages.GROUP_NOT_FOUND);
        }

        var student = await _context.Users
            .Where(u => u.Id == studentId)
            .SingleOrDefaultAsync(cancellationToken);

        if (student is null)
        {
            return new NotFoundError(UserErrorMessages.USER_WITH_ID_NOT_FOUND);
        }

        group.Students.Remove(student);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }

    public async Task<bool> IsInGroupAsync(int courseId, string groupName, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Groups
            .Where(g => g.CourseId == courseId && g.Name == groupName)
            .Where(g => g.Teachers.Select(u => u.Id).Contains(userId)
                        || g.Students.Select(u => u.Id).Contains(userId))
            .AnyAsync(cancellationToken);
    }

    public async Task<bool> IsCreatorAsync(int courseId, string groupName, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Groups
            .Where(g => g.CourseId == courseId && g.Name == groupName && g.CreatorId == userId)
            .AnyAsync(cancellationToken);
    }

    public async Task<bool> IsTeacherAsync(int courseId, string groupName, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Groups
            .Where(g => g.CourseId == courseId && g.Name == groupName)
            .SelectMany(g => g.Teachers.Select(u => u.Id))
            .ContainsAsync(userId, cancellationToken);
    }

    public async Task<bool> IsInGroupAsync(int groupId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Groups
            .Where(g => g.GroupId == groupId)
            .Where(g => g.Teachers.Select(u => u.Id).Contains(userId)
                        || g.Students.Select(u => u.Id).Contains(userId))
            .AnyAsync(cancellationToken);
    }

    public async Task<bool> IsCreatorAsync(int groupId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Groups
            .Where(g => g.GroupId == groupId && g.CreatorId == userId)
            .AnyAsync(cancellationToken);
    }

    public async Task<bool> IsTeacherAsync(int groupId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Groups
            .Where(g => g.GroupId == groupId)
            .SelectMany(g => g.Teachers.Select(u => u.Id))
            .ContainsAsync(userId, cancellationToken);
    }

    private async Task<Group?> GetByNameAsync(int courseId, string groupName, CancellationToken cancellationToken = default)
    {
        return await _context.Groups
            .Where(g => g.CourseId == courseId && g.Name == groupName)
            .SingleOrDefaultAsync(cancellationToken);
    }

    private async Task<Result> UpdateAsync(Group group, UpdatedGroup updatedGroup, CancellationToken cancellationToken = default)
    {
        group.Name = updatedGroup.Name;
        group.Description = updatedGroup.Description;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}