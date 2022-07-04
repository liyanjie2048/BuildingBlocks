using System.Collections.Generic;
using System.Linq;

namespace System;

/// <summary>
/// 
/// </summary>
public static class EnumHelper
{
    /// <summary>
    /// 获取枚举类型的值的集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEnumerable<T> GetItems<T>() where T : Enum
    {
        return Enum.GetValues(typeof(T)).Cast<T>();
    }
}
