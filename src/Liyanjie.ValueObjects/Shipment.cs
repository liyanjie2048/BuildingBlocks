namespace Liyanjie.ValueObjects;

/// <summary>
/// 
/// </summary>
public class Shipment<TIdentity> : ValueObject
{
    /// <summary>
    /// 标识
    /// </summary>
    public TIdentity? Identity { get; set; }

    /// <summary>
    /// 运单号码
    /// </summary>
    public string? TrackingNumber { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Identity;
        yield return TrackingNumber;
    }

    public override string ToString() => $"{Identity} {TrackingNumber}";
}

/// <summary>
/// 
/// </summary>
public class Delivery : Shipment<string>
{
}
