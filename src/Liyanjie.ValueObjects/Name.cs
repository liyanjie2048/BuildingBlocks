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
    /// 姓
    /// </summary>
    public string? Surname { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return GivenName!;
        yield return Surname!;
    }
}
