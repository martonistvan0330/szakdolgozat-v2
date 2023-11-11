using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.Constants.Errors.Course;
using HomeworkManager.Model.Contexts;
using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.Course;
using HomeworkManager.Model.CustomEntities.User;
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

    public async Task<bool> ExistsAsync(int courseId)
    {
        return await _context.Courses.AnyAsync(c => c.CourseId == courseId);
    }

    public async Task<CourseModel?> GetModelAsync(int courseId)
    {
        return await _context.Courses
            .Select(c => new CourseModel
            {
                CourseId = c.CourseId,
                Name = c.Name,
                Description = c.Description
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
            .SingleOrDefaultAsync();

        if (user is null)
        {
            return null;
        }

        return new CourseModel
        {
            CourseId = course.CourseId,
            Name = course.Name,
            Description = course.Description
        };
    }

    public async Task<IEnumerable<UserListRow>> GetTeachersAsync(int courseId)
    {
        return await _context.Courses
            .Where(c => c.CourseId == courseId)
            .SelectMany(c => c.Teachers)
            .Select(u => new UserListRow
            {
                UserId = u.Id,
                FullName = u.FullName,
                Username = u.UserName!,
                Email = u.Email!,
                Roles = ""
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<UserListRow>> GetStudentsAsync(int courseId)
    {
        return await _context.Groups
            .Where(c => c.CourseId == courseId)
            .SelectMany(c => c.Students)
            .Select(u => new UserListRow
            {
                UserId = u.Id,
                FullName = u.FullName,
                Username = u.UserName!,
                Email = u.Email!,
                Roles = ""
            })
            .ToListAsync();
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

    public async Task AddTeachersAsync(int courseId, ICollection<Guid> userIds)
    {
        var course = await _context.Courses
            .Include(c => c.Teachers)
            .SingleOrDefaultAsync(c => c.CourseId == courseId);

        if (course is not null)
        {
            var teachers = await _context.Users
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync();

            course.Teachers = course.Teachers.UnionBy(teachers, u => u.Id).ToHashSet();

            await _context.SaveChangesAsync();
        }
    }

    public async Task AddStudentsAsync(int courseId, ICollection<Guid> userIds)
    {
        var course = await _context.Courses
            .Include(c => c.Students)
            .SingleOrDefaultAsync(c => c.CourseId == courseId);

        if (course is not null)
        {
            var students = await _context.Users
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync();

            course.Students = course.Students.UnionBy(students, u => u.Id).ToHashSet();

            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> IsInCourseAsync(int courseId, Guid userId)
    {
        return await _context.Courses
            .Where(c => c.CourseId == courseId)
            .Where(c => c.Teachers.Select(u => u.Id).Contains(userId)
                        || c.Students.Select(u => u.Id).Contains(userId))
            .AnyAsync();
    }

    public async Task<bool> IsCreatorAsync(int courseId, Guid userId)
    {
        return await _context.Courses.AnyAsync(c => c.CourseId == courseId && c.CreatorId == userId);
    }

    public async Task<bool> IsTeacherAsync(int courseId, Guid userId)
    {
        return await _context.Courses
            .Where(c => c.CourseId == courseId)
            .SelectMany(c => c.Teachers.Select(u => u.Id))
            .ContainsAsync(userId);
    }

    public async Task<bool> ExistsWithNameAsync(string name)
    {
        return await _context.Courses.AnyAsync(c => c.Name == name);
    }
}