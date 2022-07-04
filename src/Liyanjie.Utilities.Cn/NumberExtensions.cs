using System.Text.RegularExpressions;

namespace System;

/// <summary>
/// 数值类型的扩展方法
/// </summary>
public static class NumberExtensions
{
    /// <summary>
    /// 将数字转换成中文表示形式
    /// </summary>
    /// <param name="number"></param>
    /// <param name="type">Normal:中文数字,Currency:中文货币,Direct:仅转中文</param>
    /// <returns></returns>
    public static string ToCn(this short number, CnNumberType type) => ToCn<long>(number, type);

    /// <summary>
    /// 将数字转换成中文表示形式
    /// </summary>
    /// <param name="number"></param>
    /// <param name="type">Normal:中文数字,Currency:中文货币,Direct:仅转中文</param>
    /// <returns></returns>
    public static string ToCn(this int number, CnNumberType type) => ToCn<long>(number, type);

    /// <summary>
    /// 将数字转换成中文表示形式
    /// </summary>
    /// <param name="number"></param>
    /// <param name="type">Normal:中文数字,Currency:中文货币,Direct:仅转中文</param>
    /// <returns></returns>
    public static string ToCn(this long number, CnNumberType type) => ToCn<long>(number, type);

    /// <summary>
    /// 将数字转换成中文表示形式
    /// </summary>
    /// <param name="number"></param>
    /// <param name="type">Normal:中文数字,Currency:中文货币,Direct:仅转中文</param>
    /// <returns></returns>
    public static string ToCn(this float number, CnNumberType type) => ToCn<double>(number, type);

    /// <summary>
    /// 将数字转换成中文表示形式
    /// </summary>
    /// <param name="number"></param>
    /// <param name="type">Normal:中文数字,Currency:中文货币,Direct:仅转中文</param>
    /// <returns></returns>
    public static string ToCn(this double number, CnNumberType type) => ToCn<double>(number, type);

    /// <summary>
    /// 将数字转换成中文表示形式
    /// </summary>
    /// <param name="number"></param>
    /// <param name="type">Normal:中文数字,Currency:中文货币,Direct:仅转中文</param>
    /// <returns></returns>
    public static string ToCn(this decimal number, CnNumberType type) => ToCn<decimal>(number, type);

    static string ToCn<T>(T number, CnNumberType type)
    {
        return type switch
        {
            CnNumberType.Number => ConvertToCnNumber(number),
            CnNumberType.Currency => ConvertToCnCurrency(number),
            CnNumberType.Digit => ConvertToCnDigit(number),
            _ => throw new ArgumentException("T must be short|int|long|decimal|float|double", nameof(number))
        };
    }
    const string format_number = "#C#B#A#O#C#B#A#N#C#B#A#M#C#B#A#L#C#B#A#K#C#B#A#J#C#B#A#I#C#B#A#H#C#B#A#G#C#B#A#F#C#B#A#E#C#B#A#D#C#B#A#.#a#b#c#d#e#f#g#";
    static string ConvertToCnNumber<T>(T number)
    {
        var s = number switch
        {
            short or int or long => Convert.ToInt64(number).ToString(format_number),
            float or double => Convert.ToDouble(number).ToString(format_number),
            decimal => Convert.ToDecimal(number).ToString(format_number),
            _ => throw new ArgumentException(),
        };
        s = Regex.Replace(s, @"(((?<=-)|(?!-)^)[^1-9]*)|((?'z'0)[0A-C|a-g]*((?=[1-9])|(?'-z'(?=[D-O\.]|$))))|((?'b'[D-O])(?'z'0)[0A-R]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
        return Regex.Replace(s, ".", _ => "负又-〇一二三四五六七八九-------十百千万亿兆京垓秭穰沟涧正载极-----------------分釐毫丝忽微纤"[_.Value[0] - 45].ToString());
    }
    const string format_currency = "#C#B#A#O#C#B#A#N#C#B#A#M#C#B#A#L#C#B#A#K#C#B#A#J#C#B#A#I#C#B#A#H#C#B#A#G#C#B#A#F#C#B#A#E#C#B#A#D#C#B#A#.#a#b#c#";
    static string ConvertToCnCurrency<T>(T number)
    {
        var s = number switch
        {
            short or int or long => Convert.ToInt64(number).ToString(format_currency),
            float or double => Convert.ToDouble(number).ToString(format_currency),
            decimal => Convert.ToDecimal(number).ToString(format_currency),
            _ => throw new ArgumentException(),
        };
        s = Regex.Replace(s, @"(((?<=-)|(?!-)^)[^1-9]*)|((?'z'0)[0A-C|a-g]*((?=[1-9])|(?'-z'(?=[D-O\.]|$))))|((?'b'[D-O])(?'z'0)[0A-R]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
        return Regex.Replace(s, ".", _ => "负圆-零壹贰叁肆伍陆柒捌玖-------拾佰仟万亿兆京垓秭穰沟涧正载极-----------------角分厘"[_.Value[0] - 45].ToString());
    }
    static string ConvertToCnDigit<T>(T number)
    {
        var s = number?.ToString();
        return Regex.Replace(s, ".", _ => "负点-〇一二三四五六七八九"[_.Value[0] - 45].ToString());
    }
}

public enum CnNumberType
{
    /// <summary>
    /// 12345.6789 &gt;&gt; 一万二千三百四十五又六分七釐八毫九丝
    /// </summary>
    Number,

    /// <summary>
    /// 12345.6789 &gt;&gt; 壹万贰仟叁佰肆拾伍圆陆角柒分捌厘玖
    /// </summary>
    Currency,

    /// <summary>
    /// 12345.6789 &gt;&gt; 一二三四五点六七八九
    /// </summary>
    Digit,
}