namespace Liyanjie.ValueObjects;

/// <summary>
/// 地址
/// </summary>
public class Address : ValueObject
{
    /// <summary>
    /// 行政区划代码
    /// </summary>
    [DisallowNull]
    public string ADCode { get; set; } = default!;

    /// <summary>
    /// 行政区划名称
    /// </summary>
    public string? ADDisplay { get; set; }

    /// <summary>
    /// 详细地址
    /// </summary>
    public string? Detail { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return ADCode;
        yield return Detail;
    }

    public override string ToString() => Detail!;
}
