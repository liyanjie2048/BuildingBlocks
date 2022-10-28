namespace Liyanjie.ValueObjects;

/// <summary>
/// 收件人
/// </summary>
public class Consignee : ValueObject
{
    /// <summary>
    /// 地址
    /// </summary>
    public Address? Address { get; set; }

    /// <summary>
    /// 电话
    /// </summary>
    public Contact? Contact { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Address!;
        yield return Contact!;
    }

    public override string ToString() => $"{Contact} {Address}";
}
