using System;

namespace Liyanjie.AspNetCore.Extensions;

/// <summary>
/// 配合DelimitedArrayModelBinderProvider使用
/// 
/// [DelimitedArray(Delimiter = ",")]
/// public string[] ModelProperty { get; set; }
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
public class DelimitedArrayAttribute : Attribute
{
    /// <summary>
    /// 分隔符，默认为“,”
    /// </summary>
    public string Delimiter { get; set; } = ",";
}
