using FluentResults;
using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.Constants.Errors.Course;
using HomeworkManager.Model.Contexts;
using HomeworkManager.Model.CustomEntities.Course;
using HomeworkManager.Model.CustomEntities.Errors;
using HomeworkManager.Model.CustomEntities.User;
using HomeworkManager.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomeworkManager.DataAccess.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly HomeworkManagerContext _context;

    public CourseRepository(HomeworkManagerContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsWithIdAsync(int courseId, CancellationToken cancellationToken = default)
    {
        return await _context.Courses
            .Where(c => c.CourseId == courseId)
            .AnyAsync(cancellationToken);
    }

    public async Task<bool> ExistsWithNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Courses
            .Where(c => c.Name == name)
            .AnyAsync(cancellationToken);
    }

    public async Task<string?> GetNameByIdAsync(int courseId, CancellationToken cancellationToken = default)
    {
        return await _context.Courses
            .Where(c => c.CourseId == courseId)
            .Select(c => c.Name)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<Course?> GetByIdAsync(int courseId, CancellationToken cancellationToken = default)
    {
        return await _context.Courses
            .Where(c => c.CourseId == courseId)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<CourseModel?> GetModelAsync(int courseId, CancellationToken cancellationToken = default)
    {
        return await _context.Courses
            .Where(c => c.CourseId == courseId)
            .Select(c => new CourseModel
            {
                CourseId = c.CourseId,
                Name = c.Name,
                Description = c.Description
            })
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<CourseModel?> GetModelAsync(int courseId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Courses
            .Where(c => c.CourseId == courseId)
            .Where(c => c.Teachers.Select(u => u.Id).Contains(userId)
                        || c.Students.Select(u => u.Id).Contains(userId))
            .Select(c => new CourseModel
            {
                CourseId = c.CourseId,
                Name = c.Name,
                Description = c.Description
            })
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<CourseCard>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Courses
            .Select(c => new CourseCard
            {
                CourseId = c.CourseId,
                Name = c.Name
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CourseCard>> GetAllAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var managedCourses = await _context.Users
            .Where(u => u.Id == userId)
            .SelectMany(u => u.ManagedCourses)
            .Select(c => new CourseCard
            {
                CourseId = c.CourseId,
                Name = c.Name
            })
            .ToListAsync(cancellationToken);

        var attendedCourses = await _context.Users
            .Where(u => u.Id == userId)
            .SelectMany(u => u.AttendedCourses)
            .Select(c => new CourseCard
            {
                CourseId = c.CourseId,
                Name = c.Name
            })
            .ToListAsync(cancellationToken);

        return managedCourses.UnionBy(attendedCourses, c => c.CourseId);
    }

    public async Task<IEnumerable<UserListRow>> GetTeachersAsync(int courseId, CancellationToken cancellationToken = default)
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
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<UserListRow>> GetStudentsAsync(int courseId, CancellationToken cancellationToken = default)
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
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CreateAsync(NewCourse newCourse, User user, CancellationToken cancellationToken = default)
    {
        Course course = new()
        {
            Name = newCourse.Name,
            Description = newCourse.Description,
            CreatorId = user.Id
        };

        _context.Courses.Add(course);
        course.Teachers.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        return course.CourseId;
    }

    public async Task<Result> UpdateAsync(int courseId, UpdatedCourse updatedCourse, CancellationToken cancellationToken = default)
    {
        var course = await GetByIdAsync(courseId, cancellationToken);

        if (course is null)
        {
            return new BusinessError(CourseErrorMessages.COURSE_NOT_FOUND);
        }

        return await UpdateAsync(course, updatedCourse, cancellationToken);
    }

    public async Task<Result> UpdateAsync(int courseId, UpdatedCourse updatedCourse, Guid userId, CancellationToken cancellationToken = default)
    {
        var course = await GetByIdAsync(courseId, cancellationToken);

        if (course is null)
        {
            return new BusinessError(CourseErrorMessages.COURSE_NOT_FOUND);
        }

        if (course.CreatorId != userId)
        {
            return new ForbiddenError();
        }

        return await UpdateAsync(course, updatedCourse, cancellationToken);
    }

    public async Task<Result> AddTeachersAsync(int courseId, IEnumerable<Guid> userIds, CancellationToken cancellationToken = default)
    {
        var course = await _context.Courses
            .Where(c => c.CourseId == courseId)
            .Include(c => c.Teachers)
            .SingleOrDefaultAsync(cancellationToken);

        if (course is null)
        {
            return new NotFoundError(CourseErrorMessages.COURSE_NOT_FOUND);
        }

        var teachers = await _context.Users
            .Where(u => userIds.Contains(u.Id))
            .ToListAsync(cancellationToken);

        course.Teachers = course.Teachers.UnionBy(teachers, u => u.Id).ToHashSet();

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }

    public async Task<Result> AddStudentsAsync(int courseId, IEnumerable<Guid> userIds, CancellationToken cancellationToken = default)
    {
        var course = await _context.Courses
            .Where(c => c.CourseId == courseId)
            .Include(c => c.Students)
            .SingleOrDefaultAsync(cancellationToken);

        if (course is null)
        {
            return new NotFoundError(CourseErrorMessages.COURSE_NOT_FOUND);
        }

        var students = await _context.Users
            .Where(u => userIds.Contains(u.Id))
            .ToListAsync(cancellationToken);

        course.Students = course.Students.UnionBy(students, u => u.Id).ToHashSet();

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }

    public async Task<bool> IsInCourseAsync(int courseId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Courses
            .Where(c => c.CourseId == courseId)
            .Where(c => c.Teachers.Select(u => u.Id).Contains(userId)
                        || c.Students.Select(u => u.Id).Contains(userId))
            .AnyAsync(cancellationToken);
    }

    public async Task<bool> IsCreatorAsync(int courseId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Courses
            .Where(c => c.CourseId == courseId && c.CreatorId == userId)
            .AnyAsync(cancellationToken);
    }

    public async Task<bool> IsTeacherAsync(int courseId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Courses
            .Where(c => c.CourseId == courseId)
            .SelectMany(c => c.Teachers.Select(u => u.Id))
            .ContainsAsync(userId, cancellationToken);
    }

    private async Task<Result> UpdateAsync(Course course, UpdatedCourse updatedCourse, CancellationToken cancellationToken = default)
    {
        course.Name = updatedCourse.Name;
        course.Description = updatedCourse.Description;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}