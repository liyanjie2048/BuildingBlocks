namespace System;

/// <summary>
/// 
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// 获取当前时间是当前年中的第几周
    /// </summary>
    /// <param name="input">当前时间</param>
    /// <param name="firstDayOfWeek">一周的第一天</param>
    /// <returns></returns>
    public static int WeekOfYear(this DateTime input, DayOfWeek firstDayOfWeek = DayOfWeek.Sunday)
    {
        return input.WeekOfYear(CultureInfo.CurrentCulture, firstDayOfWeek);
    }

    /// <summary>
    /// 获取当前时间是当前年中的第几周
    /// </summary>
    /// <param name="input">当前时间</param>
    /// <param name="culture"></param>
    /// <param name="firstDayOfWeek">一周的第一天</param>
    /// <returns></returns>
    public static int WeekOfYear(this DateTime input, CultureInfo culture, DayOfWeek firstDayOfWeek = DayOfWeek.Sunday)
    {
        return culture.Calendar.GetWeekOfYear(input, culture.DateTimeFormat.CalendarWeekRule, firstDayOfWeek);
    }

#if NET6_0_OR_GREATER
    public static DateOnly ToDateOnly(this DateTime input)
    {
        return DateOnly.FromDateTime(input);
    }

    public static TimeOnly ToTimeOnly(this DateTime input)
    {
        return TimeOnly.FromDateTime(input);
    }
#endif
}
