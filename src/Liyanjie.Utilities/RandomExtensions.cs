namespace System;

/// <summary>
/// 
/// </summary>
public static class RandomExtensions
{
    /// <summary>
    /// 随机的Boolean值
    /// </summary>
    /// <param name="random"></param>
    /// <returns></returns>
    public static bool NextBool(this Random random)
    {
        return random.NextDouble() < 0.5D;
    }
}
