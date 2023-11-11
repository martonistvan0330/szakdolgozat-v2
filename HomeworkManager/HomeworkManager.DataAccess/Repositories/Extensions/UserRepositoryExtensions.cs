using HomeworkManager.Model.Entities;

namespace HomeworkManager.DataAccess.Repositories.Extensions;

public static class UserRepositoryExtensions
{
    public static IQueryable<User> FullSearch(this IQueryable<User> users, string? searchText = null)
    {
        if (searchText is null)
        {
            return users;
        }

        return users.Where(u => u.Id.ToString().Contains(searchText)
                                || u.FullName.Contains(searchText)
                                || u.UserName!.Contains(searchText)
                                || u.Email!.Contains(searchText));
    }

    public static IQueryable<User> Search(this IQueryable<User> users, string? searchText = null)
    {
        if (searchText is null)
        {
            return users;
        }

        return users.Where(u => u.FullName.Contains(searchText) || u.Email!.Contains(searchText));
    }
}