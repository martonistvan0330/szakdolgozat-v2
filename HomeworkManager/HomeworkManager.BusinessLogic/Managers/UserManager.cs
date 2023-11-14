using System.Collections;
using FluentResults;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.BusinessLogic.Services.Authentication.Interfaces;
using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.Constants.Errors.User;
using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.Errors;
using HomeworkManager.Model.CustomEntities.User;
using HomeworkManager.Model.Entities;
using Microsoft.AspNetCore.Identity;

namespace HomeworkManager.BusinessLogic.Managers;

public class UserManager : IUserManager
{
    private readonly ICurrentUserService _currentUserService;
    private readonly UserManager<User> _identityUserManager;
    private readonly IUserRepository _userRepository;

    public UserManager
    (
        ICurrentUserService currentUserService,
        IUserRepository userRepository,
        UserManager<User> identityUserManager
    )
    {
        _currentUserService = currentUserService;
        _userRepository = userRepository;
        _identityUserManager = identityUserManager;
    }

    public async Task<bool> ExistsByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _userRepository.ExistsByIdAsync(userId, cancellationToken);
    }

    public async Task<bool> UsernameAvailableAsync(string username, CancellationToken cancellationToken = default)
    {
        return !await _userRepository.ExistsByUsernameAsync(username, cancellationToken);
    }

    public async Task<bool> EmailAvailableAsync(string email, CancellationToken cancellationToken = default)
    {
        return !await _userRepository.ExistsByEmailAsync(email, cancellationToken);
    }

    public async Task<Result<UserModel>> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var userModel = await _userRepository.GetModelByIdAsync(userId, cancellationToken);

        return userModel is not null
            ? Result.Ok(userModel)
            : Result.Fail(new NotFoundError(UserErrorMessages.USER_WITH_ID_NOT_FOUND));
    }

    public async Task<Result<UserModel>> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        var userModel = await _userRepository.GetModelByUsernameAsync(username, cancellationToken);

        return userModel is not null
            ? Result.Ok(userModel)
            : Result.Fail(new NotFoundError(UserErrorMessages.USER_WITH_NAME_NOT_FOUND));
    }

    public async Task<Guid> GetCurrentUserIdAsync(CancellationToken cancellationToken = default)
    {
        return await _currentUserService.GetIdAsync(cancellationToken);
    }

    public async Task<UserModel> GetCurrentUserModelAsync(CancellationToken cancellationToken = default)
    {
        return await _currentUserService.GetModelAsync(cancellationToken);
    }

    public async Task<bool> CurrentUserHasRoleAsync(string role, CancellationToken cancellationToken = default)
    {
        var currentUser = await _currentUserService.GetAsync(cancellationToken);

        return await _identityUserManager.IsInRoleAsync(currentUser, role);
    }

    public async Task<Pageable<UserListRow>> GetAllAsync(PageableOptions options, CancellationToken cancellationToken = default)
    {
        var totalCount = await _userRepository.GetCountAsync(options.SearchText, cancellationToken);

        var users = options.SortOptions?.Sort switch
        {
            "userId" => await _userRepository.GetAllModelsAsync(options.PageData, u => u.UserId,
                options.SortOptions.SortDirection.ToSortDirection(), options.SearchText, cancellationToken),
            "fullName" => await _userRepository.GetAllModelsAsync(options.PageData, u => u.FullName,
                options.SortOptions.SortDirection.ToSortDirection(), options.SearchText, cancellationToken),
            "username" => await _userRepository.GetAllModelsAsync(options.PageData, u => u.Username,
                options.SortOptions.SortDirection.ToSortDirection(), options.SearchText, cancellationToken),
            "email" => await _userRepository.GetAllModelsAsync(options.PageData, u => u.Email,
                options.SortOptions.SortDirection.ToSortDirection(), options.SearchText, cancellationToken),
            _ => await _userRepository.GetAllModelsAsync(options.PageData, cancellationToken)
        };

        return new Pageable<UserListRow>
        {
            Items = users,
            TotalCount = totalCount
        };
    }

    public async Task<Result> UpdateRoles(Guid userId, ICollection<int> roleIds, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

        if (user is null)
        {
            return Result.Fail(new NotFoundError(UserErrorMessages.USER_WITH_ID_NOT_FOUND));
        }
        
        var rolesToRemove = await _userRepository.GetRoleNamesToRemove(userId, roleIds, cancellationToken);
        var rolesToAdd = await _userRepository.GetRoleNamesToAdd(userId, roleIds, cancellationToken);
        
        await _identityUserManager.RemoveFromRolesAsync(user, rolesToRemove);
        await _identityUserManager.AddToRolesAsync(user, rolesToAdd);

        return Result.Ok();
    }
}