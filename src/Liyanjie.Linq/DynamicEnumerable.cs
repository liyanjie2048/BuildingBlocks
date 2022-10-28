namespace System.Linq;

/// <summary>
/// 
/// </summary>
public static class DynamicEnumerable
{
    #region ToArray

    /// <summary>
    /// 
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static object[] ToArray(this IEnumerable source)
    {
        Check.NotNull(source, nameof(source));

        return CastToArray<object>(source);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static T[] ToArray<T>(this IEnumerable source)
    {
        Check.NotNull(source, nameof(source));

        return CastToArray<T>(source);
    }

    static T[] CastToArray<T>(IEnumerable source)
    {
        return Enumerable.ToArray(source.Cast<T>());
    }

    #endregion

    #region ToList

    /// <summary>
    /// 
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static List<object> ToList(this IEnumerable source)
    {
        Check.NotNull(source, nameof(source));

        return CastToList<object>(source);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static List<T> ToList<T>(this IEnumerable source)
    {
        Check.NotNull(source, nameof(source));

        return CastToList<T>(source);
    }

    static List<T> CastToList<T>(IEnumerable source)
    {
        return Enumerable.ToList(source.Cast<T>());
    }

    #endregion
}