using System.Linq.Expressions;
using FluentResults;
using HomeworkManager.DataAccess.Repositories.Extensions;
using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.Constants;
using HomeworkManager.Model.Constants.Errors.Assignment;
using HomeworkManager.Model.Contexts;
using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.Assignment;
using HomeworkManager.Model.CustomEntities.Errors;
using HomeworkManager.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomeworkManager.DataAccess.Repositories;

public class AssignmentRepository : IAssignmentRepository
{
    private readonly HomeworkManagerContext _context;

    public AssignmentRepository(HomeworkManagerContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsWithIdAsync(int assignmentId, CancellationToken cancellationToken = default)
    {
        return await _context.Assignments
            .Where(a => a.AssignmentId == assignmentId)
            .AnyAsync(cancellationToken);
    }

    public async Task<bool> ExistsDraftWithIdAsync(int assignmentId, CancellationToken cancellationToken = default)
    {
        return await _context.Assignments
            .Where(a => a.AssignmentId == assignmentId && a.IsDraft)
            .AnyAsync(cancellationToken);
    }

    public async Task<bool> ExistsWithNameAsync(int groupId, string name, CancellationToken cancellationToken = default)
    {
        return await _context.Assignments
            .Where(a => a.GroupId == groupId && a.Name == name)
            .AnyAsync(cancellationToken);
    }

    public async Task<int> GetCountByUserIdAsync(Guid userId, string? searchText = null, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Where(u => u.Id == userId)
            .SelectMany(u => u.AttendedGroups.Union(u.ManagedGroups))
            .SelectMany(g => g.Assignments)
            .Where(a => !a.IsDraft || a.CreatorId == userId)
            .Search(searchText)
            .CountAsync(cancellationToken);
    }

    public async Task<IEnumerable<AssignmentListRow>> GetAllByUserIdAsync<TKey>(
        Guid userId,
        PageData? pageData = null,
        Expression<Func<AssignmentListRow, TKey>>? orderBy = null,
        SortDirection sortDirection = SortDirection.Ascending,
        string? searchText = null,
        CancellationToken cancellationToken = default
    )
    {
        return await _context.Users
            .Where(u => u.Id == userId)
            .SelectMany(u => u.AttendedGroups.Union(u.ManagedGroups))
            .SelectMany(g => g.Assignments)
            .Where(a => !a.IsDraft || a.CreatorId == userId)
            .Search(searchText)
            .ToListModel()
            .OrderByWithDirection(orderBy, sortDirection)
            .GetPage(pageData)
            .ToListAsync(cancellationToken);
    }

    public async Task<AssignmentModel?> GetModelByIdAsync(int assignmentId, CancellationToken cancellationToken = default)
    {
        return await _context.Assignments
            .Where(a => a.AssignmentId == assignmentId)
            .ToModel()
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<string?> GetNameByIdAsync(int assignmentId, CancellationToken cancellationToken = default)
    {
        return await _context.Assignments
            .Where(a => a.AssignmentId == assignmentId)
            .Select(a => a.Name)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<int?> GetGroupIdByIdAsync(int assignmentId, CancellationToken cancellationToken = default)
    {
        return await _context.Assignments
            .Where(a => a.AssignmentId == assignmentId)
            .Select(a => a.GroupId)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<int> CreateAsync(NewAssignment newAssignment, int groupId, Guid creatorId, CancellationToken cancellationToken = default)
    {
        var deadline = DateTime.Now.Date.AddDays(14).AddHours(23).AddMinutes(59).AddSeconds(59);

        Assignment assignment = new()
        {
            Name = newAssignment.Name,
            GroupId = groupId,
            CreatorId = creatorId,
            Deadline = deadline
        };

        _context.Assignments.Add(assignment);
        await _context.SaveChangesAsync(cancellationToken);

        return assignment.AssignmentId;
    }

    public async Task<Result> UpdateAsync(int assignmentId, UpdatedAssignment updatedAssignment, CancellationToken cancellationToken = default)
    {
        var assignment = await GetByIdAsync(assignmentId, cancellationToken);

        if (assignment is null)
        {
            return new BusinessError(AssignmentErrorMessages.ASSIGNMENT_WITH_ID_NOT_FOUND);
        }

        return await UpdateAsync(assignment, updatedAssignment, cancellationToken);
    }

    public async Task<Result> UpdateAsync(int assignmentId, UpdatedAssignment updatedAssignment, Guid userId,
        CancellationToken cancellationToken = default)
    {
        var assignment = await GetByIdAsync(assignmentId, cancellationToken);

        if (assignment is null)
        {
            return new BusinessError(AssignmentErrorMessages.ASSIGNMENT_WITH_ID_NOT_FOUND);
        }

        if (assignment.CreatorId != userId)
        {
            return new ForbiddenError();
        }

        return await UpdateAsync(assignment, updatedAssignment, cancellationToken);
    }

    public async Task<Result> PublishAsync(int assignmentId, Guid userId, CancellationToken cancellationToken = default)
    {
        var assignment = await GetByIdAsync(assignmentId, cancellationToken);

        if (assignment is null)
        {
            return new BusinessError(AssignmentErrorMessages.ASSIGNMENT_WITH_ID_NOT_FOUND);
        }

        if (assignment.CreatorId != userId)
        {
            return new ForbiddenError();
        }

        if (assignment.AssignmentTypeId is null)
        {
            return new BusinessError(AssignmentErrorMessages.ASSIGNMENT_TYPE_REQUIRED);
        }

        // if (assignment.Description is null)
        // {
        //     return new BusinessError(AssignmentErrorMessages.DESCRIPTION_REQUIRED);
        // }

        var twoWeeks = DateTime.Now.Date.AddDays(14);

        if (assignment.Deadline.Date < twoWeeks)
        {
            return new BusinessError(AssignmentErrorMessages.INVALID_DEADLINE);
        }

        assignment.IsDraft = false;
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }

    public async Task<bool> IsCreatorAsync(int assignmentId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Assignments
            .Where(a => a.AssignmentId == assignmentId && a.CreatorId == userId)
            .AnyAsync(cancellationToken);
    }

    public async Task<AssignmentTypeId?> GetAssignmentTypeIdAsync(int assignmentId, CancellationToken cancellationToken = default)
    {
        return await _context.Assignments
            .Where(a => a.AssignmentId == assignmentId)
            .Select(a => a.AssignmentTypeId)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<AssignmentType>> GetAssignmentTypes(CancellationToken cancellationToken = default)
    {
        return await _context.AssignmentTypes.ToListAsync(cancellationToken);
    }

    private async Task<Assignment?> GetByIdAsync(int assignmentId, CancellationToken cancellationToken = default)
    {
        return await _context.Assignments
            .Where(a => a.AssignmentId == assignmentId)
            .SingleOrDefaultAsync(cancellationToken);
    }

    private async Task<Result> UpdateAsync(Assignment assignment, UpdatedAssignment updatedAssignment, CancellationToken cancellationToken = default)
    {
        var deadline = updatedAssignment.Deadline.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

        assignment.Name = updatedAssignment.Name;
        assignment.Description = updatedAssignment.Description;
        assignment.Deadline = deadline;
        assignment.AssignmentTypeId = updatedAssignment.AssignmentTypeId;
        assignment.PresentationRequired = updatedAssignment.PresentationRequired;

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Ok();
    }
}