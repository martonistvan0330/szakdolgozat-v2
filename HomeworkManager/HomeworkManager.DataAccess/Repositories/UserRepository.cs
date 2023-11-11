﻿using System.Linq.Expressions;
using HomeworkManager.DataAccess.Repositories.Extensions;
using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.Contexts;
using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.Role;
using HomeworkManager.Model.CustomEntities.User;
using HomeworkManager.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomeworkManager.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly HomeworkManagerContext _context;

    public UserRepository(HomeworkManagerContext context)
    {
        _context = context;
    }

    public async Task<UserModel?> GetModelByIdAsync(Guid userId)
    {
        var user = await _context.Users.Where(u => u.Id == userId).SingleOrDefaultAsync();

        return await CreateModelAsync(user);
    }

    public async Task<UserModel?> GetModelByNameAsync(string username)
    {
        var user = await _context.Users.Where(u => u.UserName == username).SingleOrDefaultAsync();

        return await CreateModelAsync(user);
    }

    public async Task<IEnumerable<UserListRow>> GetAllModelsAsync<TKey>
    (
        PageData? pageData = null,
        Expression<Func<UserListRow, TKey>>? orderBy = null,
        SortDirection sortDirection = SortDirection.Ascending,
        string? searchText = null
    )
    {
        return await _context.Users
            .FullSearch(searchText)
            .Join
            (
                _context.UserRoles,
                u => u.Id,
                ur => ur.UserId,
                (u, ur) => new
                {
                    UserId = u.Id,
                    Username = u.UserName,
                    u.FullName,
                    u.Email,
                    ur.RoleId
                }
            )
            .Join
            (
                _context.Roles,
                u => u.RoleId,
                r => r.Id,
                (u, r) => new
                {
                    u.UserId,
                    u.FullName,
                    u.Username,
                    u.Email,
                    RoleName = r.Name
                }
            )
            .GroupBy(u => new { u.UserId, u.FullName, u.Username, u.Email })
            .Select(g => new UserListRow
            {
                FullName = g.Key.FullName,
                UserId = g.Key.UserId,
                Username = g.Key.Username!,
                Email = g.Key.Email!,
                Roles = string.Join(", ", g.Select(ur => ur.RoleName))
            })
            .OrderByWithDirection(orderBy, sortDirection)
            .GetPage(pageData)
            .ToListAsync();
    }

    public async Task<int> GetCountAsync(string? searchText = null)
    {
        return await _context.Users.FullSearch(searchText).CountAsync();
    }

    private async Task<UserModel?> CreateModelAsync(User? user)
    {
        if (user is null)
        {
            return null;
        }

        var roles = await _context.UserRoles
            .Where(ur => ur.UserId == user.Id)
            .Join(
                _context.Roles,
                ur => ur.RoleId,
                r => r.Id,
                (ur, r) => new RoleModel
                {
                    RoleId = r.RoleId,
                    Name = r.Name!
                }
            )
            .ToListAsync();

        return new UserModel
        {
            UserId = user.Id,
            FullName = user.FullName,
            Username = user.UserName!,
            Email = user.Email!,
            EmailConfirmed = user.EmailConfirmed,
            Roles = roles
        };
    }
}