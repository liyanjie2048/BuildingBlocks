namespace System.Linq.Expressions;

/// <summary>
/// 
/// </summary>
public static class ExpressionExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <param name="concat"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Expression<Func<T, bool>> If_AndAlso<T>(this Expression<Func<T, bool>> source,
        bool predicate,
        Expression<Func<T, bool>> concat)
    {
        if (predicate && concat is not null)
            return source.AndAlso(concat);

        return source;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <param name="concat"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Expression<Func<T, bool>> If_OrElse<T>(this Expression<Func<T, bool>> source,
        bool predicate,
        Expression<Func<T, bool>> concat)
    {
        if (predicate && concat is not null)
            return source.OrElse(concat);

        return source;
    }

    /// <summary>
    /// AndAlso
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="concat"></param>
    /// <returns></returns>
    public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> source,
        Expression<Func<T, bool>> concat)
        => Expression.Lambda<Func<T, bool>>(Expression.AndAlso(source.Body, ReplaceParameters(concat.Body, GetMappings(concat.Parameters, source.Parameters))), source.Parameters);

    /// <summary>
    /// OrElse
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="concat"></param>
    /// <returns></returns>
    public static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> source,
        Expression<Func<T, bool>> concat)
        => Expression.Lambda<Func<T, bool>>(Expression.OrElse(source.Body, ReplaceParameters(concat.Body, GetMappings(concat.Parameters, source.Parameters))), source.Parameters);

    static Dictionary<ParameterExpression, ParameterExpression> GetMappings(
        ReadOnlyCollection<ParameterExpression> parameters1,
        ReadOnlyCollection<ParameterExpression> parameters2,
        Func<ParameterExpression, int, KeyValuePair<ParameterExpression, ParameterExpression>>? map = null)
    {
        map ??= ((_, i) => new(_, parameters2[i]));
        return parameters1
            .Select(map)
            .ToDictionary(_ => _.Key, _ => _.Value);
    }

    static Expression ReplaceParameters(
        Expression expression,
        Dictionary<ParameterExpression, ParameterExpression> mappings)
    {
        return new ExpressionVisitor(mappings).Visit(expression);
    }

    sealed class ExpressionVisitor(
        Dictionary<ParameterExpression, ParameterExpression> mappings)
        : Expressions.ExpressionVisitor
    {
        readonly Dictionary<ParameterExpression, ParameterExpression> mappings = mappings ?? [];

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (mappings.TryGetValue(node, out var value))
                node = value;
            return base.VisitParameter(node);
        }
    }
}
