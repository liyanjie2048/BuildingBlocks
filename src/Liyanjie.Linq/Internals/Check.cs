namespace Liyanjie.Linq.Internals;

internal static class Check
{
    public static void Condition<T>(T value, Predicate<T> condition, string parameterName)
    {
        NotNull(condition, nameof(condition));
        NotNull(value, nameof(value));

        if (!condition(value))
        {
            NotEmpty(parameterName, nameof(parameterName));

            throw new ArgumentOutOfRangeException(parameterName);
        }
    }

    public static void NotNull<T>(T value, string parameterName)
    {
        if (value is null)
        {
            NotEmpty(parameterName, nameof(parameterName));

            throw new ArgumentNullException(parameterName);
        }
    }

    public static void NotNull<T>(T value, string parameterName, string propertyName)
    {
        if (value is null)
        {
            NotEmpty(parameterName, nameof(parameterName));
            NotEmpty(propertyName, nameof(propertyName));

            throw new ArgumentException(ArgumentPropertyNull(propertyName, parameterName));
        }
    }

    public static void NotEmpty<T>(IList<T> value, string parameterName)
    {
        NotNull(value, parameterName);

        if (value.Count == 0)
        {
            NotEmpty(parameterName, nameof(parameterName));

            throw new ArgumentException(CollectionArgumentIsEmpty(parameterName));
        }
    }

    public static void NotEmpty(string value, string parameterName)
    {
        Exception? e = null;
        if (value is null)
        {
            e = new ArgumentNullException(parameterName);
        }
        else if (value.Trim().Length == 0)
        {
            e = new ArgumentException(ArgumentIsEmpty(parameterName));
        }

        if (e != null)
        {
            NotEmpty(parameterName, nameof(parameterName));

            throw e;
        }
    }

    public static void NullButNotEmpty(string value, string parameterName)
    {
        if (value is not null && (value.Length == 0))
        {
            NotEmpty(parameterName, nameof(parameterName));

            throw new ArgumentException(ArgumentIsEmpty(parameterName));
        }
    }

    public static void HasNoNulls<T>(IList<T> value, string parameterName) where T : class
    {
        NotNull(value, parameterName);

        if (value.Any(_ => _ is null))
        {
            NotEmpty(parameterName, nameof(parameterName));

            throw new ArgumentException(parameterName);
        }
    }

    public static void ValidEntityType(Type type, string parameterName)
    {
        if (!type.GetTypeInfo().IsClass)
        {
            NotEmpty(parameterName, nameof(parameterName));

            throw new ArgumentException(invalidEntityType(type, parameterName));
        }
    }

    /// <summary>
    /// The property '{property}' of the argument '{argument}' cannot be null.
    /// </summary>
    static string ArgumentPropertyNull(string property, string argument)
    {
        return string.Format(CultureInfo.CurrentCulture, $"The property '{property}' of the argument '{argument}' cannot be null.", property, argument);
    }

    /// <summary>
    /// The string argument '{argumentName}' cannot be empty.
    /// </summary>
    static string ArgumentIsEmpty(string argumentName)
    {
        return string.Format(CultureInfo.CurrentCulture, $"The string argument '{argumentName}' cannot be empty.", argumentName);
    }

    /// <summary>
    /// The entity type '{type}' provided for the argument '{argumentName}' must be a reference type.
    /// </summary>
    static string invalidEntityType(Type type, string argumentName)
    {
        return string.Format(CultureInfo.CurrentCulture, $"The entity type '{type}' provided for the argument '{argumentName}' must be a reference type.", type, argumentName);
    }

    /// <summary>
    /// The collection argument '{argumentName}' must contain at least one element.
    /// </summary>
    static string CollectionArgumentIsEmpty(string argumentName)
    {
        return string.Format(CultureInfo.CurrentCulture, $"The collection argument '{argumentName}' must contain at least one element.", argumentName);
    }
}