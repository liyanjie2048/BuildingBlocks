namespace Liyanjie.Utilities;

/// <summary>
/// 
/// </summary>
public static class Consts
{
    /// <summary>
    /// IPv4正则
    /// </summary>
    public const string RegexPattern_IPv4 = @"([1-9]?\d|1\d{2}|2([0-4]\d|5[0-5]))(\.([1-9]?\d|1\d{2}|2([0-4]\d|5[0-5]))){3}";

    /// <summary>
    /// E-Mail正则
    /// </summary>
    public const string RegexPattern_Email = @"(\w)+(\.\w+)*@(\w)+((\.\w+)+)";
}
