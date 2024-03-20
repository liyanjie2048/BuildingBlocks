namespace Liyanjie.Linq.Expressions.Internal;

/// <summary>
/// 
/// </summary>
internal static class Extentions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="char"></param>
    /// <returns></returns>
    public static CharId Id(this char @char)
    {
        if (char.IsLetter(@char))
            return CharId.Letter;
        if (char.IsDigit(@char))
            return CharId.Digit;
        return @char switch
        {
            ' ' or '\0' or '\r' or '\n' => CharId.Empty,
            '!' => CharId.Exclam,
            '"' => CharId.DoubleQuote,
            '#' => CharId.Sharp,
            '$' => CharId.Dollar,
            '%' => CharId.Modulo,
            '&' => CharId.And,
            '\'' => CharId.SingleQuote,
            '(' => CharId.LeftParenthesis,
            ')' => CharId.RightParenthesis,
            '*' => CharId.Asterisk,
            '+' => CharId.Plus,
            ',' => CharId.Comma,
            '-' => CharId.Minus,
            '.' => CharId.Dot,
            '/' => CharId.Slash,
            ':' => CharId.Colon,
            ';' => CharId.Semicolon,
            '<' => CharId.LessThan,
            '=' => CharId.Equal,
            '>' => CharId.GreaterThan,
            '?' => CharId.Question,
            '@' => CharId.At,
            '[' => CharId.LeftBracket,
            '\\' => CharId.Backslash,
            ']' => CharId.RightBracket,
            '^' => CharId.Caret,
            '_' => CharId.Underline,
            '`' => CharId.Backquote,
            '{' => CharId.LeftBrace,
            '|' => CharId.Bar,
            '}' => CharId.RightBrace,
            '~' => CharId.Tilde,
            _ => CharId.Unknow,
        };
    }

    public static string Description(this Enum @enum)
    {
        var type = @enum.GetType();
        var name = Enum.GetName(type, @enum)!;

        return type.GetField(name)?.GetCustomAttribute<DescriptionAttribute>(false)?.Description ?? name;
    }

    public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> source, Func<T, bool> separatorSelector)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        if (separatorSelector is null)
            throw new ArgumentNullException(nameof(source));

        var output = new List<IEnumerable<T>>();
        var i = -1;
        var length = source.Count();
        for (int j = 0; j < length; j++)
        {
            if (separatorSelector(source.ElementAt(j)))
            {
                var item = source.Skip(i + 1).Take(j - i - 1);
                if (item.Count() > 0)
                    output.Add(item);
                i = j;
            }
        }
        if (i < length - 1)
        {
            var item = source.Skip(i + 1).ToList();
            if (item.Count() > 0)
                output.Add(item);
        }

        return output;
    }
}
