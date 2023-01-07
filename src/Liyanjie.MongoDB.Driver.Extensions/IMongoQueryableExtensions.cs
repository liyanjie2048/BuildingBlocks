namespace MongoDB.Driver.Linq;

public static class IMongoQueryableExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="ifPredicate"></param>
    /// <param name="wherePredicate"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IMongoQueryable<TSource> IfWhere<TSource>(this IMongoQueryable<TSource> source,
        Func<bool> ifPredicate,
        Expression<Func<TSource, bool>> wherePredicate)
    {
        if (ifPredicate is null)
            throw new ArgumentNullException(nameof(ifPredicate));

        if (wherePredicate is null)
            throw new ArgumentNullException(nameof(wherePredicate));

        return ifPredicate.Invoke()
            ? source.Where(wherePredicate)
            : source;
    }
}
