namespace Liyanjie.ValueObjects;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TIndustry"></typeparam>
public class Job<TIndustry> : ValueObject
{
    /// <summary>
    /// 行业
    /// </summary>
    public TIndustry? Industry { get; set; }

    /// <summary>
    /// 公司
    /// </summary>
    public string? Company { get; set; }

    /// <summary>
    /// 职位
    /// </summary>
    public string? Position { get; set; }

    /// <summary>
    /// 地址
    /// </summary>
    public Address? Address { get; set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Industry!;
        yield return Company!;
        yield return Position!;
        yield return Address!;
    }

    public override string ToString() => $"{Company} {Position}";
}
public class Job : Job<string> { }
