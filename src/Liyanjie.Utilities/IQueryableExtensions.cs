namespace System.Linq;

/// <summary>
/// 
/// </summary>
public static class IQueryableExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="ifPredicate"></param>
    /// <param name="wherePredicate"></param>
    /// <returns></returns>
    public static IQueryable<TSource> IfWhere<TSource>(this IQueryable<TSource> source,
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
