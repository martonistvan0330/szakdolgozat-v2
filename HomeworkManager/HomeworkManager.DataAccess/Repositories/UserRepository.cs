using System.Linq.Expressions;
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

    public async Task<bool> ExistsByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Where(u => u.Id == userId)
            .AnyAsync(cancellationToken);
    }

    public async Task<int> GetCountAsync(string? searchText = null, CancellationToken cancellationToken = default)
    {
        return await _context.Users.FullSearch(searchText).CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Where(u => u.UserName == username)
            .AnyAsync(cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Where(u => u.Email == email)
            .AnyAsync(cancellationToken);
    }

    public async Task<UserModel?> GetModelByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await GetModelAsync(u => u.Id == userId, cancellationToken);
    }

    public async Task<IEnumerable<UserListRow>> GetAllModelsAsync<TKey>
    (
        PageData? pageData = null,
        Expression<Func<UserListRow, TKey>>? orderBy = null,
        SortDirection sortDirection = SortDirection.Ascending,
        string? searchText = null,
        CancellationToken cancellationToken = default
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
            .ToListAsync(cancellationToken);
    }

    public async Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Where(u => u.Id == userId)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Where(u => u.UserName == username)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<Guid?> GetIdByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Where(u => u.UserName == username)
            .Select(u => u.Id)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<UserModel?> GetModelByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await GetModelAsync(u => u.UserName == username, cancellationToken);
    }

    public async Task<UserModel?> GetModelByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await GetModelAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<bool> HasRoleByIdAsync(string roleName, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Where(u => u.Id == userId)
            .Join
            (
                _context.UserRoles,
                u => u.Id,
                ur => ur.UserId,
                (u, ur) => ur.RoleId
            )
            .Join
            (
                _context.Roles,
                roleId => roleId,
                r => r.Id,
                (roleId, r) => r.Name
            )
            .ContainsAsync(roleName, cancellationToken);
    }

    public async Task<IEnumerable<string>> GetRoleNamesToAdd(Guid userId, IEnumerable<int> roleIds, CancellationToken cancellationToken = default)
    {
        var currentRoleNames = await GetCurrentRoleNames(userId, cancellationToken);
        var roleNames = await GetRoleNames(roleIds, cancellationToken);
        return roleNames.Except(currentRoleNames);
    }

    public async Task<IEnumerable<string>> GetRoleNamesToRemove(Guid userId, IEnumerable<int> roleIds, CancellationToken cancellationToken = default)
    {
        var currentRoleNames = await GetCurrentRoleNames(userId, cancellationToken);
        var roleNames = await GetRoleNames(roleIds, cancellationToken);
        return currentRoleNames.Except(roleNames);
    }

    public async Task<bool> ConfirmEmailAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await GetByIdAsync(userId, cancellationToken);

        if (user is null || user.EmailConfirmed)
        {
            return false;
        }

        user.EmailConfirmed = true;
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task RevokeAccessTokensAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var accessTokens = _context.Users
            .Where(u => u.Id == userId)
            .SelectMany(u => u.AccessTokens)
            .Where(at => at.IsActive);

        foreach (var accessToken in accessTokens)
        {
            accessToken.IsActive = false;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task<IEnumerable<string>> GetCurrentRoleNames(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Where(u => u.Id == userId)
            .Join
            (
                _context.UserRoles,
                u => u.Id,
                ur => ur.UserId,
                (u, ur) => new
                {
                    ur.RoleId
                }
            )
            .Join
            (
                _context.Roles,
                ur => ur.RoleId,
                r => r.Id,
                (ur, r) => r.Name!
            )
            .Distinct()
            .ToListAsync(cancellationToken);
    }

    private async Task<IEnumerable<string>> GetRoleNames(IEnumerable<int> roleIds, CancellationToken cancellationToken = default)
    {
        return await _context.Roles
            .Where(r => roleIds.Contains(r.RoleId))
            .Select(r => r.Name!)
            .Distinct()
            .ToListAsync(cancellationToken);
    }

    private async Task<UserModel?> GetModelAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Where(predicate)
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
                    u.EmailConfirmed,
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
                    u.EmailConfirmed,
                    r.RoleId,
                    RoleName = r.Name
                }
            )
            .GroupBy(u => new { u.UserId, u.FullName, u.Username, u.Email, u.EmailConfirmed })
            .Select(g => new UserModel
            {
                UserId = g.Key.UserId,
                FullName = g.Key.FullName,
                Username = g.Key.Username!,
                Email = g.Key.Email!,
                EmailConfirmed = g.Key.EmailConfirmed,
                Roles = g.Select(ur => new RoleModel
                {
                    RoleId = ur.RoleId,
                    Name = ur.RoleName!
                }).ToHashSet()
            })
            .SingleOrDefaultAsync(cancellationToken);
    }
}