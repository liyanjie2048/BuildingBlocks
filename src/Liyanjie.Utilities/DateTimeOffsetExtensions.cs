namespace System;

/// <summary>
/// 
/// </summary>
public static class DateTimeOffsetExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static DateTimeOffset Date(this DateTimeOffset input) => input.Date;

    /// <summary>
    /// 获取当前时间是当前年中的第几周
    /// </summary>
    /// <param name="input">当前时间</param>
    /// <param name="firstDayOfWeek">一周的第一天</param>
    /// <returns></returns>
    public static int WeekOfYear(this DateTimeOffset input, DayOfWeek firstDayOfWeek = DayOfWeek.Sunday)
    {
        return input.DateTime.WeekOfYear(firstDayOfWeek);
    }

    /// <summary>
    /// 获取当前时间是当前年中的第几周
    /// </summary>
    /// <param name="input">当前时间</param>
    /// <param name="culture"></param>
    /// <param name="firstDayOfWeek">一周的第一天</param>
    /// <returns></returns>
    public static int WeekOfYear(this DateTimeOffset input, CultureInfo culture, DayOfWeek firstDayOfWeek = DayOfWeek.Sunday)
    {
        return input.DateTime.WeekOfYear(culture, firstDayOfWeek);
    }

#if NET6_0_OR_GREATER
    public static DateOnly ToDateOnly(this DateTimeOffset input)
    {
        return DateOnly.FromDateTime(input.DateTime);
    }

    public static TimeOnly ToTimeOnly(this DateTimeOffset input)
    {
        return TimeOnly.FromDateTime(input.DateTime);
    }
#endif
}
