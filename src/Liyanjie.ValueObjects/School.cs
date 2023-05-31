namespace Liyanjie.ValueObjects;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TType"></typeparam>
public class School<TType> : ValueObject
{
    /// <summary>
    /// 类型
    /// </summary>
    public TType? Type { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 入校时间
    /// </summary>
    public DateTime? AdmissionDate { get; set; }

    /// <summary>
    /// 毕业时间
    /// </summary>
    public DateTime? GraduatedDate { get; set; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Type;
        yield return Name;
    }

    public override string? ToString() => Type?.ToString();
}
public class School : School<string> { }
