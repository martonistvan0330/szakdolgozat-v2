﻿using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.Constants.Errors.Group;
using HomeworkManager.Model.Contexts;
using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.Group;
using HomeworkManager.Model.Entities;
using HomeworkManager.Model.ErrorEntities;
using Microsoft.EntityFrameworkCore;

namespace HomeworkManager.DataAccess.Repositories;

public class GroupRepository : IGroupRepository
{
    private readonly HomeworkManagerContext _context;

    public GroupRepository(HomeworkManagerContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<GroupListRow>> GetAllAsync(int courseId)
    {
        return await _context.Groups
            .Where(g => g.CourseId == courseId)
            .Select(g => new GroupListRow
            {
                GroupId = g.GroupId,
                Name = g.Name
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<GroupListRow>> GetAllByUserAsync(int courseId, Guid userId)
    {
        var user = await _context.Users
            .Include(u => u.AttendedCourses)
            .Include(u => u.ManagedCourses)
            .SingleOrDefaultAsync(u => u.Id == userId);

        if (user is null)
        {
            return new HashSet<GroupListRow>();
        }

        return await _context.Groups
            .Where(g => g.CourseId == courseId)
            .Where(g => g.Teachers.Select(u => u.Id).Contains(userId)
                        || g.Students.Select(u => u.Id).Contains(userId))
            .Select(g => new GroupListRow
            {
                GroupId = g.GroupId,
                Name = g.Name
            })
            .ToListAsync();
    }

    public async Task<bool> ExistsWithNameAsync(string groupName, int courseId)
    {
        return await _context.Groups.AnyAsync(g => g.CourseId == courseId && g.Name == groupName);
    }

    public async Task<GroupModel?> GetModelAsync(int courseId, string groupName)
    {
        return await _context.Groups
            .Where(g => g.CourseId == courseId && g.Name == groupName)
            .Select(g => new GroupModel
            {
                Name = g.Name,
                Description = g.Description
            })
            .SingleOrDefaultAsync();
    }

    public async Task<GroupModel?> GetModelByUserAsync(int courseId, string groupName, Guid userId)
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
            .SingleOrDefaultAsync();
    }

    public async Task<Result<int, BusinessError>> CreateAsync(NewGroup newGroup, int courseId, User user)
    {
        if (await _context.Courses.Select(g => g.Name).ContainsAsync(newGroup.Name))
        {
            return new BusinessError(GroupErrorMessages.GROUP_NAME_NOT_AVAILABLE);
        }

        Group group = new()
        {
            Name = newGroup.Name,
            Description = newGroup.Description,
            CourseId = courseId,
            CreatorId = user.Id
        };

        _context.Groups.Add(group);
        group.Teachers.Add(user);
        await _context.SaveChangesAsync();

        return group.GroupId;
    }
    
    public async Task<BusinessError?> UpdateAsync(int courseId, string groupName, UpdateGroup updatedGroup, User? user = null)
    {
        var group = await _context.Groups.SingleOrDefaultAsync(g => g.CourseId == courseId && g.Name == groupName);

        if (group is null || (user is not null && group.CreatorId != user.Id))
        {
            return new BusinessError(GroupErrorMessages.GROUP_NOT_FOUND);
        }

        if (group.Name != updatedGroup.Name)
        {
            if (await _context.Courses.Select(c => c.Name).ContainsAsync(updatedGroup.Name))
            {
                return new BusinessError(GroupErrorMessages.GROUP_NAME_NOT_AVAILABLE);
            }
        }

        group.Name = updatedGroup.Name;
        group.Description = updatedGroup.Description;

        await _context.SaveChangesAsync();

        return null;
    }

    public async Task<bool> IsInGroupAsync(string groupName, int courseId, Guid userId)
    {
        return await _context.Groups
            .Where(g => g.CourseId == courseId && g.Name == groupName)
            .Where(g => g.Teachers.Select(u => u.Id).Contains(userId)
                        || g.Students.Select(u => u.Id).Contains(userId))
            .AnyAsync();
    }

    public async Task<bool> IsCreatorAsync(string groupName, int courseId, Guid userId)
    {
        return await _context.Groups
            .AnyAsync(g => g.CourseId == courseId && g.Name == groupName && g.CreatorId == userId);
    }

    public async Task<bool> IsTeacherAsync(string groupName, int courseId, Guid userId)
    {
        return await _context.Groups
            .Where(g => g.CourseId == courseId && g.Name == groupName)
            .SelectMany(g => g.Teachers.Select(u => u.Id))
            .ContainsAsync(userId);
    }
}