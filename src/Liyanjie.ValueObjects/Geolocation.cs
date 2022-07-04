using System.Collections.Generic;

namespace Liyanjie.ValueObjects;

/// <summary>
/// 位置
/// </summary>
public class Geolocation : ValueObject
{
    /// <summary>
    /// 所在经度
    /// </summary>
    public double Longitude { get; set; }

    /// <summary>
    /// 所在纬度
    /// </summary>
    public double Latitude { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Longitude;
        yield return Latitude;
    }

    public override string ToString() => $"{Longitude:0.000000},{Latitude:0.000000}";
}
