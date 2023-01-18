namespace Liyanjie.ValueObjects;

/// <summary>
/// 证件
/// </summary>
public class Licence<TType> : ValueObject
{
    /// <summary>
    /// 类型
    /// </summary>
    public TType? Type { get; set; }

    /// <summary>
    /// 号码
    /// </summary>        
    public string? Number { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 照片
    /// </summary>
    public string[]? Pictures { get; set; }

    /// <summary>
    /// 认证状态
    /// </summary>
    public Status<bool> IsIdentified { get; set; } = new();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Type!;
        yield return Number!;
    }

    public override string ToString() => $"{Name} {Number}";
}

/// <summary>
/// 证件
/// </summary>
public class Licence : Licence<string> { }
