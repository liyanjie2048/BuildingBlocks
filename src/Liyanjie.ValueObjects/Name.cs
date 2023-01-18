namespace Liyanjie.ValueObjects;

/// <summary>
/// 
/// </summary>
public class Name : ValueObject
{
    /// <summary>
    /// 名
    /// </summary>
    public string? GivenName { get; set; }

    /// <summary>
    /// 中间名
    /// </summary>
    public string? MiddleName { get; set; }

    /// <summary>
    /// 姓
    /// </summary>
    public string? FamilyName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return GivenName;
        yield return MiddleName;
        yield return FamilyName;
    }
}
