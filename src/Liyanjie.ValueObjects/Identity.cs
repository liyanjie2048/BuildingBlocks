namespace Liyanjie.ValueObjects;

/// <summary>
/// Identity
/// </summary>
public class Identity<TType, TValue> : ValueObject
{
    /// <summary>
    /// 类型
    /// </summary>
    [DisallowNull]
    public TType Type { get; set; } = default!;

    /// <summary>
    /// 标识
    /// </summary>
    public TValue Value { get; set; } = default!;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Type;
        yield return Value;
    }

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
public class Identity : Identity<string, string> { }
