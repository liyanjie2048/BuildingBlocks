namespace System;

#if NET6_0_OR_GREATER

public static class DateOnlyExtensions
{
    public static DateTime ToDateTime(this DateOnly date)
    {
        return date.ToDateTime(TimeOnly.MinValue);
    }
}

#endif