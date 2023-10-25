using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.User;

namespace HomeworkManager.BusinessLogic.Managers.Interfaces;

public interface IUserManager
{
    Task<UserModel?> GetByIdAsync(Guid userId);
    Task<UserModel?> GetByNameAsync(string username);
    Task<Pageable<UserListRow>> GetAllAsync(SortOptions sortOptions, PageData pageData);
}