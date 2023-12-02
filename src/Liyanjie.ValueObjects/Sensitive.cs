namespace Liyanjie.ValueObjects;

/// <summary>
/// 敏感数据
/// </summary>
/// <typeparam name="TValue"></typeparam>
/// <typeparam name="TStatus"></typeparam>
public class Sensitive<TValue, TStatus> : ValueObject
{
    public TValue? Value { get; set; }

    /// <summary>
    /// 变更时间
    /// </summary>
    public DateTimeOffset ChangeTime { get; set; } = DateTimeOffset.Now;

    /// <summary>
    /// 
    /// </summary>
    public TStatus? Status { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }

    public override string? ToString() => Value?.ToString();
}

/// <summary>
/// 
/// </summary>
public class Sensitive : Sensitive<string, bool> { }
