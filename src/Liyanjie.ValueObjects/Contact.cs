namespace Liyanjie.ValueObjects;

/// <summary>
/// 联系人
/// </summary>
public class Contact<TType, TName> : ValueObject
{
    /// <summary>
    /// 联系方式
    /// </summary>
    public TType? Type { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    [DisallowNull]
    public TName Name { get; set; } = default!;

    /// <summary>
    /// 号码
    /// </summary>
    [DisallowNull]
    public string Number { get; set; } = default!;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Type;
        yield return Name;
        yield return Number;
    }

    public override string ToString() => $"{Name} {Number}";
}

/// <summary>
/// 
/// </summary>
public class Contact : Contact<string, string> { }
