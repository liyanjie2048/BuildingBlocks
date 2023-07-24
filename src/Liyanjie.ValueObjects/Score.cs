namespace Liyanjie.ValueObjects;

/// <summary>
/// 评分
/// </summary>
public class Score : ValueObject
{
    /// <summary>
    /// 评分
    /// </summary>
    public double Total { get; set; }

    /// <summary>
    /// 评分数量
    /// </summary>
    public uint Count { get; set; }

    /// <summary>
    /// 平均分
    /// </summary>
    public double Average => Count == 0 ? 0 : Total / Count;

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
    public string ToString(string format) => Average.ToString(format);
}
