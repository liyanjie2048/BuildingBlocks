namespace Liyanjie.Linq.Expressions.Exceptions;

/// <summary>
/// 
/// </summary>
public class ExpressionParseException : Exception
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    public ExpressionParseException(string message)
        : base(message) { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="input"></param>
    /// <param name="token"></param>
    internal ExpressionParseException(string message, string? input, Token token)
        : base(
$@"{message}，在索引位置“{token.Index}”：…
{(token.Index - 7 >= 0 ? input?.Substring(token.Index - 7, 7) : input?[..token.Index])}
`{input?.Substring(token.Index, token.Length)}`"
              )
    { }
}
