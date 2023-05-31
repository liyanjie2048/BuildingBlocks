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
    public double? Average => Count > 0 ? (double)Total / Count : null;

    public void Add(int score)
    {
        Total += score;
        Count++;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Total;
        yield return Count;
    }

    public override string ToString() => Average.ToString();
    public string? ToString(string format) => Average?.ToString(format);
}
