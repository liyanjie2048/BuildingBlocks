namespace Liyanjie.ValueObjects;

/// <summary>
/// 状态
/// </summary>
/// <typeparam name="TStatus"></typeparam>
public class Status<TStatus> : ValueObject
{
    /// <summary>
    /// 值
    /// </summary>
    public TStatus? Value { get; set; }

    /// <summary>
    /// 变更时间
    /// </summary>
    public DateTimeOffset ChangeTime { get; set; } = DateTimeOffset.Now;

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value!;
    }

    public override string? ToString() => Value?.ToString();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="status"></param>
    /// <param name="remark"></param>
    /// <returns></returns>
    public static Status<TStatus> Create(TStatus status, string? remark = default) => new()
    {
        Value = status,
        Remark = remark,
    };
}

/// <summary>
/// 
/// </summary>
public class Status : Status<string> { }
