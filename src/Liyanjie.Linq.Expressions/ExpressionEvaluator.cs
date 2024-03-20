namespace Liyanjie.Linq.Expressions;

/// <summary>
/// 
/// </summary>
public static class ExpressionEvaluator
{
    static readonly ParameterExpression _parameterExpression = Expression.Parameter(typeof(object));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static object? Evaluate(string input)
    {
        var parser = new ExpressionParser(_parameterExpression, null);
        var expression = parser.Parse(input);
        var function = Expression.Lambda(expression).Compile();
        var result = function.DynamicInvoke();
        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="input">表达式字符串</param>
    /// <param name="variables">变量字典</param>
    /// <returns></returns>
    public static object? Evaluate(string input, ref Dictionary<string, object?> variables)
    {
        var parser = new ExpressionParser(_parameterExpression, variables);
        var expression = parser.Parse(input);
        var function = Expression.Lambda(expression).Compile();
        var result = function.DynamicInvoke();
        variables = parser.Variables;
        return result;
    }
}
