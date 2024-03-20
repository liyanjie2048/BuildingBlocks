namespace Liyanjie.Linq.Internals;

internal static class ExtendMethods
{

    #region CreateQuery

    public static IQueryable CreateQuery(this IQueryable source, MethodInfo method)
    {
        if (method.IsGenericMethod)
            method = method.MakeGenericMethod(source.ElementType);

        var expression = Expression.Call(method, source.Expression);
        return source.Provider.CreateQuery(expression);
    }

    public static IQueryable CreateQuery(this IQueryable source, MethodInfo method, ConstantExpression constant)
    {
        if (method.IsGenericMethod)
            method = method.MakeGenericMethod(source.ElementType);

        var expression = Expression.Call(method, source.Expression, constant);
        return source.Provider.CreateQuery(expression);
    }

    public static IQueryable CreateQuery(this IQueryable source, MethodInfo method, LambdaExpression lambda)
    {
        method = method.GetGenericArguments().Length == 2
            ? method.MakeGenericMethod(source.ElementType, lambda.Body.Type)
            : method.MakeGenericMethod(source.ElementType);

        var expression = Expression.Call(method, source.Expression, lambda);
        return source.Provider.CreateQuery(expression);
    }

    #endregion

    #region Execute

    /// <summary>
    /// 用于：First、FirstOrDefault、Last、LastOrDefault、Single、SingleOrDefault
    /// </summary>
    /// <param name="source"></param>
    /// <param name="method"></param>
    /// <param name="lambda"></param>
    /// <returns></returns>
    public static object? Execute(this IQueryable source, MethodInfo method, LambdaExpression? lambda = null)
    {
        if (method.IsGenericMethod)
            method = method.MakeGenericMethod(source.ElementType);

        var expression = lambda is null
            ? Expression.Call(method, source.Expression)
            : Expression.Call(method, source.Expression, lambda);
        return source.Provider.Execute(expression);
    }

    /// <summary>
    /// 用于：All、Any、Count
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="source"></param>
    /// <param name="method"></param>
    /// <param name="lambda"></param>
    /// <returns></returns>
    public static TResult Execute<TResult>(this IQueryable source, MethodInfo method, LambdaExpression? lambda = null)
    {
        if (method.IsGenericMethod)
            method = method.MakeGenericMethod(source.ElementType);

        var expression = lambda is null
            ? Expression.Call(method, source.Expression)
            : Expression.Call(method, source.Expression, lambda);
        return source.Provider.Execute<TResult>(expression);
    }

    /// <summary>
    /// 用于：ElementAt、ElementAtOrDefault
    /// </summary>
    /// <param name="source"></param>
    /// <param name="method"></param>
    /// <param name="constant"></param>
    /// <returns></returns>
    public static object? Execute(this IQueryable source, MethodInfo method, ConstantExpression constant)
    {
        if (method.IsGenericMethod)
            method = method.MakeGenericMethod(source.ElementType);

        var expression = Expression.Call(method, source.Expression, constant);
        return source.Provider.Execute(expression);
    }

    /// <summary>
    /// 用于：Max、Min
    /// </summary>
    /// <param name="source"></param>
    /// <param name="method"></param>
    /// <param name="lambda"></param>
    /// <returns></returns>
    public static object? ExecuteMaxMin(this IQueryable source, MethodInfo method, LambdaExpression? lambda = null)
    {
        if (method.GetGenericArguments().Length == 2 && lambda is null)
            throw new ArgumentNullException(nameof(lambda));

        method = method.GetGenericArguments().Length == 2
            ? method.MakeGenericMethod(source.ElementType, lambda!.ReturnType)
            : method.MakeGenericMethod(source.ElementType);

        var expression = lambda is null
            ? Expression.Call(method, source.Expression)
            : Expression.Call(method, source.Expression, lambda);
        return source.Provider.Execute(expression);
    }

    /// <summary>
    /// 用于：Average
    /// </summary>
    /// <param name="source"></param>
    /// <param name="lambda"></param>
    /// <returns></returns>
    public static object? ExecuteAverage(this IQueryable source, LambdaExpression? lambda = null)
    {
        var method = nameof(Queryable.Average);

        var expression = lambda is null
            ? Expression.Call(typeof(Queryable), method, null, source.Expression)
            : Expression.Call(typeof(Queryable), method, [source.ElementType], source.Expression, lambda);

        return source.Provider.Execute(expression);
    }

    /// <summary>
    /// 用于：Sum
    /// </summary>
    /// <param name="source"></param>
    /// <param name="lambda"></param>
    /// <returns></returns>
    public static object? ExecuteSum(this IQueryable source, LambdaExpression? lambda = null)
    {
        var method = nameof(Queryable.Sum);

        var expression = lambda is null
            ? Expression.Call(typeof(Queryable), method, null, source.Expression)
            : Expression.Call(typeof(Queryable), method, [source.ElementType], source.Expression, lambda);

        return source.Provider.Execute(expression);
    }

    #endregion

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    static bool IsNullableType(this Type type)
    {
        return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    static Type GetNonNullableType(this Type type)
    {
        return IsNullableType(type) ? type.GetTypeInfo().GenericTypeParameters[0] : type;
    }
}