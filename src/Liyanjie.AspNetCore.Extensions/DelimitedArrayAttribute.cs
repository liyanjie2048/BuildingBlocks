namespace Liyanjie.AspNetCore.Extensions;

/// <summary>
/// 配合DelimitedArrayModelBinderProvider使用
/// 
/// action([DelimitedArray] string[] parameter)
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
public class DelimitedArrayAttribute(
    string delimiter = ",",
    StringSplitOptions splitOptions = StringSplitOptions.RemoveEmptyEntries)
    : Attribute
{
    /// <summary>
    /// 分隔符，默认为“,”
    /// </summary>
    public string Delimiter => delimiter ?? throw new ArgumentNullException(nameof(delimiter));

    /// <summary>
    /// 
    /// </summary>
    public StringSplitOptions SplitOptions => splitOptions;
}
