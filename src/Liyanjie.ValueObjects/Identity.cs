namespace Liyanjie.ValueObjects;

/// <summary>
/// Identity
/// </summary>
public class Identity<TType, TValue> : ValueObject
{
    /// <summary>
    /// 类型
    /// </summary>
    public TType? Type { get; set; }

    /// <summary>
    /// 标识
    /// </summary>
    public TValue? Value { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Type!;
        yield return Value!;
    }

    public override string? ToString() => Type?.ToString();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Identity<TType, TValue> Create(TType type, TValue value) => new()
    {
        Type = type,
        Value = value,
    };
}

/// <summary>
/// 
/// </summary>
public class Identity : Identity<string, string>
{
}
