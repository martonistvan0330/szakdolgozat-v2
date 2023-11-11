using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.User;
using HomeworkManager.Model.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HomeworkManager.BusinessLogic.Managers;

public class UserManager : UserManager<User>, IUserManager
{
    private readonly IUserRepository _userRepository;

    public UserManager
    (
        IUserStore<User> store,
        IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<User> passwordHasher,
        IEnumerable<IUserValidator<User>> userValidators,
        IEnumerable<IPasswordValidator<User>> passwordValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        IServiceProvider services,
        ILogger<UserManager<User>> logger,
        IUserRepository userRepository
    ) : base
    (
        store,
        optionsAccessor,
        passwordHasher,
        userValidators,
        passwordValidators,
        keyNormalizer,
        errors,
        services,
        logger
    )
    {
        _userRepository = userRepository;
    }

    public async Task<UserModel?> GetByIdAsync(Guid userId)
    {
        return await _userRepository.GetModelByIdAsync(userId);
    }

    public async Task<UserModel?> GetByNameAsync(string username)
    {
        return await _userRepository.GetModelByNameAsync(username);
    }

    public async Task<Pageable<UserListRow>> GetAllAsync(PageableOptions options)
    {
        var totalCount = await _userRepository.GetCountAsync(options.SearchText);

        var users = options.SortOptions?.Sort switch
        {
            "userId" => await _userRepository.GetAllModelsAsync(options.PageData, u => u.UserId, options.SortOptions.SortDirection.ToSortDirection(),
                options.SearchText),
            "fullName" => await _userRepository.GetAllModelsAsync(options.PageData, u => u.FullName,
                options.SortOptions.SortDirection.ToSortDirection(), options.SearchText),
            "username" => await _userRepository.GetAllModelsAsync(options.PageData, u => u.Username,
                options.SortOptions.SortDirection.ToSortDirection(), options.SearchText),
            "email" => await _userRepository.GetAllModelsAsync(options.PageData, u => u.Email, options.SortOptions.SortDirection.ToSortDirection(),
                options.SearchText),
            _ => await _userRepository.GetAllModelsAsync(options.PageData)
        };

        return new Pageable<UserListRow>
        {
            Items = users,
            TotalCount = totalCount
        };
    }
}