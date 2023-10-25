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
            await _roleManager.CreateAsync(new Role { RoleId = 1, Name = Roles.ADMINISTRATOR });
        }
        else
        {
            var role = await _roleManager.FindByNameAsync(Roles.ADMINISTRATOR);

            if (role is null || role.RoleId != 1)
            {
                role!.RoleId = 1;
                await _roleManager.UpdateAsync(role!);
            }
        }

        if (!await _roleManager.RoleExistsAsync(Roles.TEACHER))
        {
            await _roleManager.CreateAsync(new Role { RoleId = 2, Name = Roles.TEACHER });
        }
        else
        {
            var role = await _roleManager.FindByNameAsync(Roles.TEACHER);

            if (role is null || role.RoleId != 2)
            {
                role!.RoleId = 2;
                await _roleManager.UpdateAsync(role!);
            }
        }

        if (!await _roleManager.RoleExistsAsync(Roles.STUDENT))
        {
            await _roleManager.CreateAsync(new Role { RoleId = 3, Name = Roles.STUDENT });
        }
        else
        {
            var role = await _roleManager.FindByNameAsync(Roles.STUDENT);

            if (role is null || role.RoleId != 3)
            {
                role!.RoleId = 3;
                await _roleManager.UpdateAsync(role!);
            }
        }
    }
}