using HomeworkManager.BusinessLogic.Services.Seed.Interfaces;
using HomeworkManager.Model.Constants;
using HomeworkManager.Model.Entities;
using Microsoft.AspNetCore.Identity;

namespace HomeworkManager.BusinessLogic.Services.Seed;

public class RoleSeedService : IRoleSeedService
{
    private readonly RoleManager<Role> _roleManager;

    public RoleSeedService(RoleManager<Role> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task SeedRolesAsync()
    {
        if (!await _roleManager.RoleExistsAsync(Roles.ADMINISTRATOR))
        {
            await _roleManager.CreateAsync(new Role { Name = Roles.ADMINISTRATOR });
        }

        if (!await _roleManager.RoleExistsAsync(Roles.TEACHER))
        {
            await _roleManager.CreateAsync(new Role { Name = Roles.TEACHER });
        }

        if (!await _roleManager.RoleExistsAsync(Roles.STUDENT))
        {
            await _roleManager.CreateAsync(new Role { Name = Roles.STUDENT });
        }
    }
}