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

    public async Task<Pageable<UserListRow>> GetAllAsync(SortOptions sortOptions, PageData pageData)
    {
        var totalCount = await _userRepository.GetCountAsync();

        var users = sortOptions.Sort switch
        {
            "userId" => await _userRepository.GetAllModelsAsync(u => u.UserId.ToString(), sortOptions.SortDirection.ToSortDirection(), pageData),
            "fullName" => await _userRepository.GetAllModelsAsync(u => u.FullName, sortOptions.SortDirection.ToSortDirection(), pageData),
            "username" => await _userRepository.GetAllModelsAsync(u => u.Username, sortOptions.SortDirection.ToSortDirection(), pageData),
            "email" => await _userRepository.GetAllModelsAsync(u => u.Email, sortOptions.SortDirection.ToSortDirection(), pageData),
            _ => await _userRepository.GetAllModelsAsync(u => u.UserId, sortOptions.SortDirection.ToSortDirection(), pageData)
        };

        return new Pageable<UserListRow>
        {
            Items = users,
            TotalCount = totalCount
        };
    }
}