namespace System.Linq;

/// <summary>
/// 
/// </summary>
public static class QueryableExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <param name="wherePredicate"></param>
    /// <returns></returns>
    public static IQueryable<TSource> If_Where<TSource>(this IQueryable<TSource> source,
        bool predicate,
        Expression<Func<TSource, bool>> wherePredicate)
    {
        if (predicate && wherePredicate is not null)
            return source.Where(wherePredicate);

        return source;
    }
}
