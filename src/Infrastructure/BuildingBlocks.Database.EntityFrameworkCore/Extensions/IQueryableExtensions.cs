using BuildingBlocks.Domain.Dto;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BuildingBlocks.Database.EntityFrameworkCore.Extensions;

public static class IQueryableExtensions
{
    public static PagedResult<T> ToPagedResult<T>(this IQueryable<T> query, ListRequest request)
        where T : notnull, new()
    {
        int totalCount = query.Count();

        if (!string.IsNullOrEmpty(request.SortBy))
        {
            var propertyInfo = typeof(T).GetProperty(request.SortBy);

            // If the property doesn't exist, return an empty result
            if (propertyInfo == null)
                return new PagedResult<T>();

            //  Build a lambda expression: x => x.PropertyName
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyInfo);
            var lambda = Expression.Lambda(property, parameter);

            string methodName = request.IsDescending ? "OrderByDescending" : "OrderBy";

            // Find the correct LINQ method via reflection
            var method = typeof(Queryable).GetMethods()
                .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), property.Type);

            // Invoke the method dynamically to apply ordering
            query = (IQueryable<T>)method.Invoke(null, [query, lambda]);
        }

        int skip = (request.PageNumber - 1) * request.PageSize;

        var items = query
            .Skip(skip)
            .Take(request.PageSize)
            .ToList();

        return new PagedResult<T>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }

    public static Task<List<T>> QueryListAsync<T>(this IQueryable<T> dbSet,
                                                                       Expression<Func<T, bool>> expression,
                                                                       CancellationToken cancellationToken = default) where T : class
    {
        return dbSet.Where(expression).ToListAsync(cancellationToken);
    }

    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query,
        bool condition,
        Expression<Func<T, bool>> predicate)
        where T : class
    {
        if (condition)
        {
            return query.Where(predicate);
        }
        return query;
    }
}
