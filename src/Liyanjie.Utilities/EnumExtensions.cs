namespace System;

/// <summary>
/// 
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// 描述
    /// </summary>
    /// <param name="enum"></param>
    /// <param name="separator"></param>
    /// <returns></returns>
    public static string? Description(this Enum @enum, char separator = ',')
    {
        var type = @enum.GetType();
        var flags = type.GetTypeInfo().GetCustomAttribute<FlagsAttribute>();

        if (flags is null)
            return GetDescription(type, Enum.GetName(type, @enum));
        else
        {
            var values = Enum.GetValues(type);
            if (values is null || values.Length == 0)
                return null;

            string? output = null;
            foreach (var value in values)
            {
                if (0.Equals(value))
                {
                    if (@enum.Equals(value))
                    {
                        output = $"{output}{separator}{GetDescription(type, Enum.GetName(type, value))}";
                        break;
                    }
                }
                else if (@enum.HasFlag((Enum)value))
                    output = $"{output}{separator}{GetDescription(type, Enum.GetName(type, value))}";
            }

            return output?.TrimStart(separator);
        }

        static string? GetDescription(Type enumType, string enumName)
        {
            if (string.IsNullOrWhiteSpace(enumName))
                return null;

            return enumType.GetField(enumName)?.GetCustomAttribute<DescriptionAttribute>(false)?.Description;
        }
    }
}
