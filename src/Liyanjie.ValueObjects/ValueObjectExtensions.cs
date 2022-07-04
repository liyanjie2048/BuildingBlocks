using System;
using System.Reflection;

namespace Liyanjie.ValueObjects;

/// <summary>
/// 
/// </summary>
public static class ValueObjectExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="origin"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static void AssignValue<T>(this T? origin, T? value)
        where T : ValueObject
    {
        if (origin is null)
            throw new ArgumentNullException(nameof(origin));

        if (value is null)
            return;

        var type = origin.GetType();
        var type_ComplexType = typeof(ValueObject);
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var property in properties)
        {
            if (type_ComplexType.IsAssignableFrom(property.PropertyType) && property.GetValue(origin) != null)
                AssignValue(property.GetValue(origin) as ValueObject, property.GetValue(value) as ValueObject);
            else if (property.CanWrite && value is not null)
                property.SetValue(origin, property.GetValue(value));
        }
    }
}
