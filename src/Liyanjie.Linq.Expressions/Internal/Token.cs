namespace Liyanjie.Linq.Expressions.Internal;

/// <summary>
/// 
/// </summary>
internal class Token
{
    /// <summary>
    /// 
    /// </summary>
    public TokenId Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int Index { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int Length { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public object Value { get; set; } = new();
}