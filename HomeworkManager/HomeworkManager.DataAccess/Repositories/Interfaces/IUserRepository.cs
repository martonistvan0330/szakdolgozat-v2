using System.Linq.Expressions;
using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.User;

namespace HomeworkManager.DataAccess.Repositories.Interfaces;

public interface IUserRepository
{
    Task<UserModel?> GetModelByIdAsync(Guid userId);
    Task<UserModel?> GetModelByNameAsync(string username);

    Task<IEnumerable<UserListRow>> GetAllModelsAsync(PageData? pageData = null)
    {
        return GetAllModelsAsync(pageData, u => u.UserId);
    }

    Task<IEnumerable<UserListRow>> GetAllModelsAsync<TKey>
    (
        PageData? pageData = null,
        Expression<Func<UserListRow, TKey>>? orderBy = null,
        SortDirection sortDirection = SortDirection.Ascending,
        string? searchText = null
    );

    Task<int> GetCountAsync(string? searchText = null);
}