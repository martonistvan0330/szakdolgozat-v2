using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.Constants.Errors.Course;
using HomeworkManager.Model.Contexts;
using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.Course;
using HomeworkManager.Model.CustomEntities.Group;
using HomeworkManager.Model.Entities;
using HomeworkManager.Model.ErrorEntities;
using Microsoft.EntityFrameworkCore;

namespace HomeworkManager.DataAccess.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly HomeworkManagerContext _context;

    public CourseRepository(HomeworkManagerContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CourseCard>> GetAllAsync()
    {
        return await _context.Courses.Select(c => new CourseCard
        {
            CourseId = c.CourseId,
            Name = c.Name
        }).ToListAsync();
    }

    public async Task<IEnumerable<CourseCard>> GetAllByUserAsync(Guid userId)
    {
        var user = await _context.Users
            .Include(u => u.AttendedCourses)
            .Include(u => u.ManagedCourses)
            .SingleOrDefaultAsync(u => u.Id == userId);

        if (user is null)
        {
            return new HashSet<CourseCard>();
        }

        return user.AttendedCourses
            .Concat(user.ManagedCourses)
            .Select(c => new CourseCard
            {
                CourseId = c.CourseId,
                Name = c.Name
            })
            .DistinctBy(c => c.CourseId);
    }
    
    public async Task<CourseModel?> GetModelAsync(int courseId)
    {
        return await _context.Courses
            .Select(c => new CourseModel
            {
                CourseId = c.CourseId,
                Name = c.Name,
                Description = c.Description,
                Groups = c.Groups.Select(g => new GroupListRow
                {
                    GroupId = g.GroupId,
                    Name = g.Name
                }).ToHashSet()
            })
            .SingleOrDefaultAsync(c => c.CourseId == courseId);
    }

    public async Task<CourseModel?> GetModelByUserAsync(int courseId, Guid userId)
    {
        var course = await _context.Courses
            .SingleOrDefaultAsync(c => c.CourseId == courseId);

        if (course is null)
        {
            return null;
        }

        var user = await _context.Users
            .Where(u => u.Id == userId)
            .Where(u => u.AttendedCourses.Select(c => c.CourseId).Contains(courseId)
                        || u.ManagedCourses.Select(c => c.CourseId).Contains(courseId))
            .Include(u => u.AttendedGroups.Where(g => g.CourseId == courseId))
            .Include(u => u.ManagedGroups.Where(g => g.CourseId == courseId))
            .SingleOrDefaultAsync();

        if (user is null)
        {
            return null;
        }

        var groups = user.AttendedGroups
            .Concat(user.ManagedGroups)
            .Select(g => new GroupListRow
            {
                GroupId = g.GroupId,
                Name = g.Name
            })
            .DistinctBy(g => g.GroupId)
            .ToHashSet();

        return new CourseModel
        {
            CourseId = course.CourseId,
            Name = course.Name,
            Description = course.Description,
            Groups = groups
        };
    }

    public async Task<Result<int, BusinessError>> CreateAsync(NewCourse newCourse, User user)
    {
        if (await _context.Courses.Select(c => c.Name).ContainsAsync(newCourse.Name))
        {
            return new BusinessError(CourseErrorMessages.COURSE_NAME_NOT_AVAILABLE);
        }

        Course course = new()
        {
            Name = newCourse.Name,
            Description = newCourse.Description,
            CreatorId = user.Id
        };

        _context.Courses.Add(course);
        course.Teachers.Add(user);
        await _context.SaveChangesAsync();

        return course.CourseId;
    }

    public async Task<BusinessError?> UpdateAsync(int courseId, UpdateCourse updatedCourse, User? user = null)
    {
        var course = await _context.Courses.SingleOrDefaultAsync(c => c.CourseId == courseId);

        if (course is null || (user is not null && course.CreatorId != user.Id))
        {
            return new BusinessError(CourseErrorMessages.COURSE_NOT_FOUND);
        }

        if (course.Name != updatedCourse.Name)
        {
            if (await _context.Courses.Select(c => c.Name).ContainsAsync(updatedCourse.Name))
            {
                return new BusinessError(CourseErrorMessages.COURSE_NAME_NOT_AVAILABLE);
            }
        }

        course.Name = updatedCourse.Name;
        course.Description = updatedCourse.Description;

        await _context.SaveChangesAsync();

        return null;
    }

    public async Task<bool> IsCreatorAsync(int courseId, Guid userId)
    {
        var course = await _context.Courses.SingleOrDefaultAsync(c => c.CourseId == courseId);

        if (course is null)
        {
            return false;
        }

        return course.CreatorId == userId;
    }

    public async Task<bool> IsTeacherAsync(int courseId, Guid userId)
    {
        return await _context.Courses
            .Where(c => c.CourseId == courseId)
            .SelectMany(c => c.Teachers.Select(u => u.Id))
            .ContainsAsync(userId);
    }

    public async Task<Course?> GetByNameOrDefaultAsync(string name)
    {
        return await _context.Courses.SingleOrDefaultAsync(c => c.Name == name);
    }
}