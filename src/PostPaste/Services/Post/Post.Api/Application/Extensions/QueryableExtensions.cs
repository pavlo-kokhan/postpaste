namespace Post.Api.Application.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> ToPageQuery<T>(this IQueryable<T> query, int page = 1, int pageSize = 10)
        => query.Skip((page - 1) * pageSize).Take(pageSize);
}