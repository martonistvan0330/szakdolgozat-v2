using Bogus;
using HomeworkManager.BusinessLogic.Managers;
using HomeworkManager.BusinessLogic.Services.Seed.Interfaces;
using HomeworkManager.Model.Constants;
using HomeworkManager.Model.CustomEntities.User;
using HomeworkManager.Model.Entities;

namespace HomeworkManager.BusinessLogic.Services.Seed;

public class UserSeedService : IUserSeedService
{
    private readonly UserManager _userManager;

    public UserSeedService(UserManager userManager)
    {
        _userManager = userManager;
    }

    public async Task SeedUsersAsync()
    {
        await SeedAdminUserAsync();

        await SeedStudentUsersAsync();
        await SeedTeacherUsersAsync();
    }

    private async Task SeedAdminUserAsync()
    {
        if (!(await _userManager.GetUsersInRoleAsync(Roles.ADMINISTRATOR)).Any())
        {
            var adminUser = new User
            {
                UserName = "admin",
                Email = "admin@homeworkmanager.aut.bme.hu",
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var createResult = await _userManager.CreateAsync(adminUser, "admin");

            if (!createResult.Succeeded)
            {
                throw new ApplicationException("Administrator could not be created: " +
                                               string.Join(", ", createResult.Errors.Select(e => e.Description)));
            }

            var addToRolesResult = await _userManager.AddToRolesAsync(
                adminUser,
                new[] { Roles.ADMINISTRATOR, Roles.TEACHER, Roles.STUDENT }
            );

            if (!addToRolesResult.Succeeded)
            {
                throw new ApplicationException("Administrator could not be created: " +
                                               string.Join(", ", addToRolesResult.Errors.Select(e => e.Description)));
            }
        }
    }

    private async Task SeedStudentUsersAsync()
    {
        var students = await _userManager.GetUsersInRoleAsync(Roles.STUDENT);
        if (students.Count < 150)
        {
            var count = 150 - students.Count;

            var newUserFaker = new Faker<NewUser>().SetUpRules();

            var newStudents = newUserFaker.Generate(count);

            foreach (var student in newStudents)
            {
                var studentUser = new User
                {
                    UserName = student.Username,
                    Email = student.Email,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                var createResult = await _userManager.CreateAsync(studentUser, student.Password);

                if (!createResult.Succeeded)
                {
                    throw new ApplicationException("Student could not be created: " +
                                                   string.Join(", ", createResult.Errors.Select(e => e.Description)));
                }

                var addToRolesResult = await _userManager.AddToRolesAsync(
                    studentUser,
                    new[] { Roles.STUDENT }
                );

                if (!addToRolesResult.Succeeded)
                {
                    throw new ApplicationException("Student could not be created: " +
                                                   string.Join(", ", addToRolesResult.Errors.Select(e => e.Description)));
                }
            }
        }
    }

    private async Task SeedTeacherUsersAsync()
    {
        var teachers = await _userManager.GetUsersInRoleAsync(Roles.TEACHER);
        if (teachers.Count < 30)
        {
            var count = 30 - teachers.Count;

            var newUserFaker = new Faker<NewUser>().SetUpRules();

            var newTeachers = newUserFaker.Generate(count);

            foreach (var teacher in newTeachers)
            {
                var teacherUser = new User
                {
                    UserName = teacher.Username,
                    Email = teacher.Email,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                var createResult = await _userManager.CreateAsync(teacherUser, teacher.Password);

                if (!createResult.Succeeded)
                {
                    throw new ApplicationException("Teacher could not be created: " +
                                                   string.Join(", ", createResult.Errors.Select(e => e.Description)));
                }

                var addToRolesResult = await _userManager.AddToRolesAsync(
                    teacherUser,
                    new[] { Roles.TEACHER }
                );

                if (!addToRolesResult.Succeeded)
                {
                    throw new ApplicationException("Teacher could not be created: " +
                                                   string.Join(", ", addToRolesResult.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}

public static class UserGenerationRules
{
    public static Faker<NewUser> SetUpRules(this Faker<NewUser> faker)
    {
        return faker
            .RuleFor(u => u.Username, f => f.Internet.UserName())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Password, f => f.Internet.Password());
    }
}