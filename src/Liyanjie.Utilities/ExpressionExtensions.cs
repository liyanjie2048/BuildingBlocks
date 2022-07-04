using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Linq.Expressions;

/// <summary>
/// 
/// </summary>
public static class ExpressionExtensions
{
    /// <summary>
    /// AndAlso
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="concat"></param>
    /// <returns></returns>
    public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> source, Expression<Func<T, bool>> concat)
        => Expression.Lambda<Func<T, bool>>(Expression.AndAlso(source.Body, ReplaceParameters(concat.Body, GetMappings(concat.Parameters, source.Parameters))), source.Parameters);

    /// <summary>
    /// OrElse
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="concat"></param>
    /// <returns></returns>
    public static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> source, Expression<Func<T, bool>> concat)
        => Expression.Lambda<Func<T, bool>>(Expression.OrElse(source.Body, ReplaceParameters(concat.Body, GetMappings(concat.Parameters, source.Parameters))), source.Parameters);

    static IDictionary<ParameterExpression, ParameterExpression> GetMappings(
        ReadOnlyCollection<ParameterExpression> parameters1,
        ReadOnlyCollection<ParameterExpression> parameters2,
        Func<ParameterExpression, int, KeyValuePair<ParameterExpression, ParameterExpression>>? map = null)
    {
        map ??= ((_, i) => new KeyValuePair<ParameterExpression, ParameterExpression>(_, parameters2[i]));
        return parameters1
            .Select(map)
            .ToDictionary(_ => _.Key, _ => _.Value);
    }

    static Expression ReplaceParameters(Expression expression, IDictionary<ParameterExpression, ParameterExpression> mappings)
    {
        return new ExpressionVisitor(mappings).Visit(expression);
    }

    sealed class ExpressionVisitor : Expressions.ExpressionVisitor
    {
        readonly IDictionary<ParameterExpression, ParameterExpression> mappings;

        public ExpressionVisitor(IDictionary<ParameterExpression, ParameterExpression> mappings)
        {
            this.mappings = mappings ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (mappings.TryGetValue(node, out var value))
                node = value;
            return base.VisitParameter(node);
        }
    }
}
