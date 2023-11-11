using System.Linq.Expressions;
using HomeworkManager.Model.CustomEntities;

namespace HomeworkManager.DataAccess.Repositories.Extensions;

public static class RepositoryExtensions
{
    public static IOrderedQueryable<TSource> OrderByWithDirection<TSource, TKey>(this IQueryable<TSource> source,
        Expression<Func<TSource, TKey>>? keySelector = null, SortDirection sortDirection = SortDirection.Ascending)
    {
        if (keySelector is null)
        {
            return source.Order();
        }
        
        return sortDirection == SortDirection.Ascending ? source.OrderBy(keySelector) : source.OrderByDescending(keySelector);
    }

    public static IQueryable<TSource> GetPage<TSource>(this IQueryable<TSource> source, PageData? pageData = null)
    {
        if (pageData is null)
        {
            return source;
        }
        
        return source.Skip(pageData.PageIndex * pageData.PageSize).Take(pageData.PageSize);
    }
}