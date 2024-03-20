namespace System.Reflection;

/// <summary>
/// 
/// </summary>
public static class DictionaryExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <param name="dictionary"></param>
    /// <returns></returns>
    public static TModel? BuildModel<TModel>(this IDictionary<string, object?> dictionary)
        where TModel : new()
    {
        return (TModel?)dictionary.BuildModel(typeof(TModel));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dictionary"></param>
    /// <param name="modelType"></param>
    /// <returns></returns>
    public static object? BuildModel(this IDictionary<string, object?> dictionary, Type modelType)
    {
        var output = Activator.CreateInstance(modelType);

        dictionary.UpdateModel(output);

        return output;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dictionary"></param>
    /// <param name="model"></param>
    public static void UpdateModel(this IDictionary<string, object?> dictionary, object? model)
    {
        if (dictionary is null || dictionary.Count == 0)
            return;

        if (model is null)
            return;

        dictionary = dictionary.ToDictionary(_ => _.Key.ToLower(), _ => _.Value);
        dictionary = PreProcessDictionary(dictionary);

        foreach (var property in model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            if (!property.CanWrite)
                continue;

            if (!dictionary.TryGetValue(property.Name.ToLower(), out var objectValue))
                continue;
            if (objectValue is null)
                property.SetValue(model, default);
            else if (property.PropertyType == objectValue.GetType())
                property.SetValue(model, objectValue);
            else if (property.PropertyType == typeof(string) && objectValue is IEnumerable enumerable)
                property.SetValue(model, string.Join(",", Enumerable.Cast<string>(enumerable)));
            else if (property.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
            {
                if (objectValue is string s)
                    objectValue = s.Split(',');

                var propertyElementType = property.PropertyType.HasElementType
                    ? property.PropertyType.GetElementType()
                    : property.PropertyType.IsConstructedGenericType
                        ? property.PropertyType.GenericTypeArguments[0]
                        : null;
                var inputArray = Enumerable.Cast<object>((IEnumerable)objectValue);
                var outputArray = Array.CreateInstance(propertyElementType ?? typeof(object), inputArray.Count());
                inputArray
                    .Select(_ => propertyElementType is null ? _ : Convert.ChangeType(_, propertyElementType))
                    .ToArray()
                    .CopyTo(outputArray, 0);
                property.SetValue(model, outputArray);
            }
            else if (property.PropertyType != typeof(string) && property.PropertyType.IsClass)
            {
                if (objectValue is IDictionary<string, object?> dic)
                {
                    var value_model = property.GetValue(model);
                    if (value_model is null)
                        property.SetValue(model, dic.BuildModel(property.PropertyType));
                    else
                    {
                        dic.UpdateModel(value_model);
                        property.SetValue(model, value_model);
                    }
                }
            }
            else
                property.SetValue(model, Convert.ChangeType(objectValue, property.PropertyType));
        }
    }

    static IDictionary<string, object?> PreProcessDictionary(IDictionary<string, object?> dictionary)
    {
        if (dictionary.Keys.Any(_ => _.IndexOf('.') > 0))
        {
            var newDictionary = dictionary
                .Where(_ => _.Key.IndexOf('.') <= 0)
                .ToDictionary(_ => _.Key, _ => _.Value);
            var subDictionaries = dictionary
                .Where(_ => _.Key.IndexOf('.') > 0)
                .GroupBy(_ => _.Key[.._.Key.IndexOf('.')]);
            foreach (var item in subDictionaries)
            {
                var subDictionary = item.ToDictionary(_ => _.Key[(item.Key.Length + 1)..], _ => _.Value);
                newDictionary[item.Key] = PreProcessDictionary(subDictionary);
            }

            return newDictionary;
        }
        else
            return dictionary;
    }
}
