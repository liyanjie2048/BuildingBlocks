namespace Liyanjie.ValueObjects;

/// <summary>
/// 敏感数据
/// </summary>
/// <typeparam name="TStatus"></typeparam>
public class Sensitive<TStatus> : ValueObject
{
    public string? Value { get; set; }

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
public class Sensitive : Sensitive<bool> { }
