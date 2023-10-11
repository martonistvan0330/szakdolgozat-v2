using HomeworkManager.BusinessLogic.Services.Seed.Interfaces;
using HomeworkManager.Model.Constants;
using HomeworkManager.Model.Entities;
using Microsoft.AspNetCore.Identity;

namespace HomeworkManager.BusinessLogic.Services.Seed
{
    public class RoleSeedService : IRoleSeedService
    {
        private readonly RoleManager<Role> _roleManager;

        public RoleSeedService(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task SeedRoleAsync()
        {
            if (!await _roleManager.RoleExistsAsync(Roles.Administrator))
            {
                await _roleManager.CreateAsync(new Role { Name = Roles.Administrator });
            }

            if (!await _roleManager.RoleExistsAsync(Roles.Teacher))
            {
                await _roleManager.CreateAsync(new Role { Name = Roles.Teacher });
            }

            if (!await _roleManager.RoleExistsAsync(Roles.Student))
            {
                await _roleManager.CreateAsync(new Role { Name = Roles.Student });
            }
        }
    }
}