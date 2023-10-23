using System.Linq.Expressions;
using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.User;

namespace HomeworkManager.DataAccess.Repositories.Interfaces;

public interface IUserRepository
{
    Task<UserModel?> GetModelByNameAsync(string username);

    Task<IEnumerable<UserListRow>> GetAllModelsAsync<TKey>
    (
        Expression<Func<UserListRow, TKey>> orderBy,
        SortDirection sortDirection,
        PageData pageData
    );

    Task<int> GetCountAsync();
}