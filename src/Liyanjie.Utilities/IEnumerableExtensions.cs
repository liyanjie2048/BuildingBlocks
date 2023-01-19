namespace System.Linq;

/// <summary>
/// 
/// </summary>
public static class IEnumerableExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="separator"></param>
    /// <param name="toString"></param>
    /// <returns></returns>
    public static string ToString<TSource>(this IEnumerable<TSource> source,
        char separator,
        Func<TSource, string?>? toString = default)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        toString ??= _ => _?.ToString();

        return string.Join(separator, source.Select(toString));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="separator"></param>
    /// <param name="toString"></param>
    /// <returns></returns>
    public static string ToString<TSource>(this IEnumerable<TSource> source,
        string separator,
        Func<TSource, string?>? toString = default)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        toString ??= _ => _?.ToString();

        return string.Join(separator, source.Select(toString));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="separatorSelector"></param>
    /// <returns></returns>
    public static IEnumerable<IEnumerable<TSource>> Split<TSource>(this IEnumerable<TSource> source,
        Func<TSource, bool> separatorSelector)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        if (separatorSelector is null)
            throw new ArgumentNullException(nameof(source));

        var output = new List<IEnumerable<TSource>>();
        var i = -1;
        var length = source.Count();
        for (int j = 0; j < length; j++)
        {
            if (separatorSelector(source.ElementAt(j)))
            {
                var item = source.Skip(i + 1).Take(j);
                if (item.Count() > 0)
                    output.Add(item);
                i = j;
            }
        }
        if (i < length - 1)
        {
            var item = source.Skip(i + 1);
            if (item.Count() > 0)
                output.Add(item);
        }

        return output;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static IEnumerable<TSource> RandomTake<TSource>(this IEnumerable<TSource> source,
        int count = 0)
    {
        if (count == 0)
            count = new Random(Guid.NewGuid().GetHashCode()).Next(source.Count());

        return source.OrderBy(_ => Guid.NewGuid()).Take(count);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <param name="wherePredicate"></param>
    /// <returns></returns>
    public static IEnumerable<TSource> If_Where<TSource>(this IEnumerable<TSource> source,
        bool predicate,
        Func<TSource, bool> wherePredicate)
    {
        if (predicate && wherePredicate is not null)
            return source.Where(wherePredicate);

        return source;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static async Task<IEnumerable<TResult>> SelectAsync<TSource, TResult>(this IEnumerable<TSource> source,
        Func<TSource, Task<TResult>> selector)
    {
        if (selector is null)
            throw new ArgumentNullException(nameof(selector));

        var output = new List<TResult>();
        foreach (var item in source)
        {
            output.Add(await selector.Invoke(item));
        }
        return output;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<IEnumerable<TResult>> SelectAsync<TSource, TResult>(this IEnumerable<TSource> source,
        Func<TSource, CancellationToken, Task<TResult>> selector,
        CancellationToken cancellationToken = default)
    {
        if (selector is null)
            throw new ArgumentNullException(nameof(selector));

        var output = new List<TResult>();
        foreach (var item in source)
        {
            output.Add(await selector.Invoke(item, cancellationToken));
        }
        return output;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static async Task<IEnumerable<TResult>> SelectManyAsync<TSource, TResult>(this IEnumerable<TSource> source,
        Func<TSource, Task<IEnumerable<TResult>>> selector)
    {
        if (selector is null)
            throw new ArgumentNullException(nameof(selector));

        var output = new List<TResult>();
        foreach (var item in source)
        {
            output.AddRange(await selector.Invoke(item));
        }
        return output;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<IEnumerable<TResult>> SelectManyAsync<TSource, TResult>(this IEnumerable<TSource> source,
        Func<TSource, CancellationToken, Task<IEnumerable<TResult>>> selector,
        CancellationToken cancellationToken = default)
    {
        if (selector is null)
            throw new ArgumentNullException(nameof(selector));

        var output = new List<TResult>();
        foreach (var item in source)
        {
            output.AddRange(await selector.Invoke(item, cancellationToken));
        }
        return output;
    }
}
