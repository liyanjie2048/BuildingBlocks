namespace Liyanjie.AspNetCore.Extensions;

/// <summary>
/// 配合DelimitedArrayModelBinderProvider使用
/// 
/// action([DelimitedArray] string[] parameter)
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
public class DelimitedArrayAttribute(string delimiter = ",") : Attribute
{
    /// <summary>
    /// 分隔符，默认为“,”
    /// </summary>
    public string Delimiter => delimiter;
}
