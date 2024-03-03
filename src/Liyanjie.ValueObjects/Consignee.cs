namespace Liyanjie.ValueObjects;

/// <summary>
/// 收件人
/// </summary>
public class Consignee : ValueObject
{
    /// <summary>
    /// 地址
    /// </summary>
    [DisallowNull]
    public Address Address { get; set; } = default!;

    /// <summary>
    /// 电话
    /// </summary>
    [DisallowNull]
    public Contact Contact { get; set; } = default!;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Address;
        yield return Contact;
    }
}
