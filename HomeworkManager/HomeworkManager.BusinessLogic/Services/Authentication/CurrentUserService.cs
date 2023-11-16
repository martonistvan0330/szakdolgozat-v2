using HomeworkManager.BusinessLogic.Services.Authentication.Interfaces;
using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.CustomEntities.User;
using HomeworkManager.Model.Entities;
using Microsoft.AspNetCore.Http;

namespace HomeworkManager.BusinessLogic.Services.Authentication;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _userRepository;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
    }

    public async Task<Guid> GetIdAsync(CancellationToken cancellationToken = default)
    {
        var username = GetUsername();

        var userId = await _userRepository.GetIdByUsernameAsync(username, cancellationToken);

        if (userId is null)
        {
            throw new ApplicationException("User not found!");
        }

        return (Guid)userId;
    }

    public async Task<User> GetAsync(CancellationToken cancellationToken = default)
    {
        var username = GetUsername();

        var user = await _userRepository.GetByUsernameAsync(username, cancellationToken);

        if (user is null)
        {
            throw new ApplicationException("User not found!");
        }

        return user;
    }

    public async Task<UserModel> GetModelAsync(CancellationToken cancellationToken = default)
    {
        var username = GetUsername();

        var userModel = await _userRepository.GetModelByUsernameAsync(username, cancellationToken);

        if (userModel is null)
        {
            throw new ApplicationException("User not found!");
        }

        return userModel;
    }

    public async Task<bool> HasRoleAsync(string roleName, CancellationToken cancellationToken = default)
    {
        var userId = await GetIdAsync(cancellationToken);

        return await _userRepository.HasRoleByIdAsync(roleName, userId, cancellationToken);
    }
    
    private string GetUsername()
    {
        var username = _httpContextAccessor.HttpContext.User.Identity?.Name;

        if (username is null)
        {
            throw new ApplicationException("No logged in user!");
        }

        return username;
    }
}