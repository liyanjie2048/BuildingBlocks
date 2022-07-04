using System;
using System.Collections.Generic;

namespace Liyanjie.ValueObjects;

/// <summary>
/// 评分
/// </summary>
public class Score : ValueObject
{
    /// <summary>
    /// 评分
    /// </summary>
    public int Total { get; set; }

    /// <summary>
    /// 评分数量
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// 平均分
    /// </summary>
    public double GetAverage(int digits = 1)
    {
        return Count > 0
            ? Math.Round(((double)Total / Count), digits, MidpointRounding.AwayFromZero)
            : 0;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Total;
        yield return Count;
    }

    public override string ToString() => GetAverage().ToString();
}
