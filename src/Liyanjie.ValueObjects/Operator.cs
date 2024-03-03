namespace Liyanjie.ValueObjects;

/// <summary>
/// 操作人
/// </summary>
/// <typeparam name="TStatus"></typeparam>
/// <typeparam name="TIdentity"></typeparam>
public class Operator<TStatus, TIdentity> : ValueObject
{
    /// <summary>
    /// 类型
    /// </summary>
    public TStatus? Status { get; set; }

    /// <summary>
    /// 标识
    /// </summary>
    public TIdentity? Identity { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Status;
        yield return Identity;
    }

    public override string? ToString() => Status?.ToString();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="status"></param>
    /// <param name="identity"></param>
    /// <returns></returns>
    public static Operator<TStatus, TIdentity> Create(TStatus status, TIdentity identity) => new()
    {
        Status = status,
        Identity = identity,
    };
}

/// <summary>
/// 
/// </summary>
public class Operator : Operator<string, string> { }
