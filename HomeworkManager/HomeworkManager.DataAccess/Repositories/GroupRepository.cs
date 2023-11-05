using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.Constants.Errors.Course;
using HomeworkManager.Model.Constants.Errors.Group;
using HomeworkManager.Model.Contexts;
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

    public async Task<BusinessError?> CreateAsync(NewGroup newGroup, int courseId, User user)
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

        return null;
    }
}