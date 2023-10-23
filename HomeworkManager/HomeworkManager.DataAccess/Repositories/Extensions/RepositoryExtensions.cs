using System.Linq.Expressions;
using HomeworkManager.Model.CustomEntities;

namespace HomeworkManager.DataAccess.Repositories.Extensions;

public static class RepositoryExtensions
{
    public static IOrderedQueryable<TSource> OrderByWithDirection<TSource, TKey>(this IQueryable<TSource> source,
        Expression<Func<TSource, TKey>> keySelector, SortDirection sortDirection)
    {
        return sortDirection == SortDirection.Ascending ? source.OrderBy(keySelector) : source.OrderByDescending(keySelector);
    }

    public static IQueryable<TSource> GetPage<TSource>(this IQueryable<TSource> source, PageData pageData)
    {
        return source.Skip(pageData.Page * pageData.PageSize).Take(pageData.PageSize);
    }
}