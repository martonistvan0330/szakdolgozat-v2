using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.Contexts;
using HomeworkManager.Model.CustomEntities.Assignment;
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

    public async Task<bool> ExistsWithNameAsync(int groupId, string name, CancellationToken cancellationToken = default)
    {
        return await _context.Assignments
            .Where(a => a.GroupId == groupId && a.Name == name)
            .AnyAsync(cancellationToken);
    }

    public async Task<int> CreateAsync(NewAssignment newAssignment, int groupId, Guid creatorId, CancellationToken cancellationToken = default)
    {
        Assignment assignment = new()
        {
            Name = newAssignment.Name,
            GroupId = groupId,
            CreatorId = creatorId
        };

        _context.Assignments.Add(assignment);
        await _context.SaveChangesAsync(cancellationToken);

        return assignment.AssignmentId;
    }
}