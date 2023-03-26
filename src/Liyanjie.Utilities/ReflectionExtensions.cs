namespace System.Reflection;

/// <summary>
/// 
/// </summary>
public static class ReflectionExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TOutput"></typeparam>
    /// <param name="input"></param>
    /// <param name="extra"></param>
    /// <returns></returns>
    public static TOutput? TranslateTo<TOutput>(this object? input, Action<TOutput>? extra = default)
    {
        var output = DoTranslate<TOutput>(input!);

        if (output is null)
            return default;

        extra?.Invoke(output);

        return output;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TOutput"></typeparam>
    /// <param name="input"></param>
    /// <param name="extra"></param>
    /// <returns></returns>
    public static async Task<TOutput?> TranslateToAsync<TOutput>(this object? input, Func<TOutput, Task> extra)
    {
        var output = DoTranslate<TOutput>(input!);

        if (output is null)
            return default;

        if (extra is not null)
            await extra.Invoke(output);

        return output;
    }

    static TOutput? DoTranslate<TOutput>(this object? input)
    {
        var translated = new Dictionary<(Type, Type, object), object>();
        var output = (TOutput?)DoTranslate(typeof(TOutput), input, translated);

        translated.Clear();

        return output;
    }

    static object? DoTranslate(Type outputType, object? input, Dictionary<(Type, Type, object), object> translated)
    {
        if (input is null)
            return null;

        var inputType = input.GetType();

        if (translated.TryGetValue((outputType, inputType, input), out object output))
            return output;

        if (outputType.IsInstanceOfType(input))
            return input;

        if (outputType.IsAssignableFrom(inputType))
            return input;

        var inputTypeInfo = inputType.GetTypeInfo();

        if (inputTypeInfo.IsEnum && (inputType == typeof(short) || inputType == typeof(ushort) || inputType == typeof(int) || inputType == typeof(uint) || inputType == typeof(long) || inputType == typeof(ulong)))
            return Convert.ChangeType(Enum.Format(inputType, input, "D"), outputType);

        var outputTypeInfo = outputType.GetTypeInfo();

        if (outputTypeInfo.IsEnum && (inputType == typeof(short) || inputType == typeof(ushort) || inputType == typeof(int) || inputType == typeof(uint) || inputType == typeof(long) || inputType == typeof(ulong)))
            return Enum.ToObject(outputType, input);

        var typeofIEnumerable = typeof(IEnumerable);
        if (typeofIEnumerable.IsAssignableFrom(outputType) && typeofIEnumerable.IsAssignableFrom(inputType))
        {
            var outputElementType = outputType.HasElementType
                ? outputType.GetElementType()
                : outputType.IsConstructedGenericType
                    ? outputType.GenericTypeArguments[0]
                    : null;
            var inputArray = Enumerable.ToArray((input as IEnumerable).Cast<object>());
            var outputArray = Array.CreateInstance(outputElementType ?? typeof(object), inputArray.Length);
            inputArray.Select(_ => outputElementType is null ? _ : DoTranslate(outputElementType, _, translated))
                .ToArray()
                .CopyTo(outputArray, 0);
            return outputArray;
        }

        if (outputType == typeof(string))
            return input?.ToString();

        if (outputTypeInfo.IsValueType)
        {
            var destType = outputType;
            if ("Nullable`1" == outputType.Name)
                destType = outputType.GenericTypeArguments[0];

            if (inputType == typeof(string))
            {
                var method = destType
                    .GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .Where(_ => _.Name == "Parse")
                    .FirstOrDefault(_ =>
                    {
                        var parameters = _.GetParameters();
                        return parameters.Length == 1 && parameters.Single().ParameterType == typeof(string);
                    });
                if (method is not null)
                    try
                    {
                        return method.Invoke(null, new[] { input });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Parse [{input}] to [{destType.Name}] error in DoTranslate");
                        Console.WriteLine(ex);
                    }
            }

            try
            {
                return Convert.ChangeType(input, destType);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ChangeType [{input}] to [{destType.Name}] error in DoTranslate");
                Console.WriteLine(ex);
            }
        }

        if (outputTypeInfo.IsInterface)
            return null;

        if (outputTypeInfo.IsAbstract)
            return null;

        output = Activator.CreateInstance(outputType);
        translated.Add((outputType, inputType, input), output);

        var inputProperties = inputType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        var outputProperties = outputType.GetProperties(BindingFlags.Instance | BindingFlags.Public);

        foreach (var outputProperty in outputProperties)
        {
            if (!outputProperty.CanWrite)
                continue;

            var inputProperty = inputProperties.FirstOrDefault(_ => _.Name == outputProperty.Name);
            if (inputProperty is null || !inputProperty.CanRead)
                continue;

            outputProperty.SetValue(output, DoTranslate(outputProperty.PropertyType, inputProperty.GetValue(input), translated));
        }

        return output;
    }

    public static void UpdateFrom(this object value, object model)
    {
        if (value is null)
            return;

        if (model is null)
            throw new ArgumentNullException(nameof(model));

        var properties_value = value.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(_ => _.CanRead);
        var type_model = model.GetType();
        foreach (var property_value in properties_value)
        {
            var property_model = type_model.GetProperty(property_value.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (property_model is null || property_model.CanWrite == false)
                continue;

            var value_ = property_value.GetValue(value);

            if (property_model.PropertyType.IsValueType && property_value.PropertyType == property_model.PropertyType)
                property_model.SetValue(model, value_);
            else if (property_model.PropertyType == typeof(string) && property_value.PropertyType == typeof(string))
                property_model.SetValue(model, value_);
            else if (property_model.PropertyType == typeof(string) && typeof(IEnumerable).IsAssignableFrom(property_value.PropertyType))
            {
                property_model.SetValue(model, value_ is null ? null : string.Join(",", Enumerable.Cast<string>((IEnumerable)value_)));
            }
            else if (property_model.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(property_model.PropertyType))
            {
                if (value_ is null)
                    property_model.SetValue(model, null);
                else
                {
                    if (value_ is string s)
                        value_ = s.Split(',');

                    var propertyElementType = property_model.PropertyType.HasElementType
                        ? property_model.PropertyType.GetElementType()
                        : property_model.PropertyType.IsConstructedGenericType
                            ? property_model.PropertyType.GenericTypeArguments[0]
                            : null;
                    var inputArray = Enumerable.Cast<object>((IEnumerable)value_);
                    var outputArray = Array.CreateInstance(propertyElementType ?? typeof(object), inputArray.Count());
                    inputArray
                        .Select(_ => propertyElementType is null ? _ : Convert.ChangeType(_, propertyElementType))
                        .ToArray()
                        .CopyTo(outputArray, 0);
                    property_model.SetValue(model, outputArray);
                }
            }
            else if (property_model.PropertyType != typeof(string) && property_model.PropertyType.IsClass)
            {
                if (property_value.PropertyType != typeof(string) && property_value.PropertyType.IsClass)
                {
                    var value_model = property_model.GetValue(model) ?? Activator.CreateInstance(property_model.PropertyType);
                    value_.UpdateFrom(value_model);
                    property_model.SetValue(model, value_model);
                }
            }
            else
                property_model.SetValue(model, Convert.ChangeType(value_, property_model.PropertyType));
        }
    }
}