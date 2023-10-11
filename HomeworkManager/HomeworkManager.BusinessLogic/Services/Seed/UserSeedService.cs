using HomeworkManager.BusinessLogic.Services.Seed.Interfaces;
using HomeworkManager.Model.Constants;
using HomeworkManager.Model.Entities;
using Microsoft.AspNetCore.Identity;

namespace HomeworkManager.BusinessLogic.Services.Seed
{
    public class UserSeedService : IUserSeedService
    {
        private readonly UserManager<User> _userManager;

        public UserSeedService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task SeedUserAsync()
        {
            if (!(await _userManager.GetUsersInRoleAsync(Roles.Administrator)).Any())
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
                    new[] { Roles.Administrator, Roles.Teacher, Roles.Student }
                );

                if (!addToRolesResult.Succeeded)
                {
                    throw new ApplicationException("Administrator could not be created: " +
                                                   string.Join(", ", addToRolesResult.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}