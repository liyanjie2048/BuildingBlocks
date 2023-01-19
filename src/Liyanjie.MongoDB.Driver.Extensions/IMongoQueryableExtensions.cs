namespace MongoDB.Driver.Linq;

public static class IMongoQueryableExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <param name="wherePredicate"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IMongoQueryable<TSource> If_Where<TSource>(this IMongoQueryable<TSource> source,
        bool predicate,
        Expression<Func<TSource, bool>> wherePredicate)
    {
        if (wherePredicate is not null)
            return predicate
                ? source.Where(wherePredicate)
                : source;

        return source;
    }
}
