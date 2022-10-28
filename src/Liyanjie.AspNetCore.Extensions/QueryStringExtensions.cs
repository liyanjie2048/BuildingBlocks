namespace Microsoft.AspNetCore.Http;

/// <summary>
/// 
/// </summary>
public static class QueryStringExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <param name="queryString"></param>
    /// <returns></returns>
    public static TModel BuildModel<TModel>(this QueryString queryString)
        where TModel : new()
    {
        if (!queryString.HasValue)
            return new();

        var dictionary = QueryHelpers.ParseNullableQuery(queryString.Value!);
        if (dictionary is null)
            return new();

        return dictionary
            .ToDictionary(_ => _.Key, _ => (object)_.Value)
            .BuildModel<TModel>();
    }
}
