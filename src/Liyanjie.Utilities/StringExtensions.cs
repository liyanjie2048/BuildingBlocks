namespace System;

/// <summary>
/// 
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// 计算长度（英文计1长度，Unicode计2长度）
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static int GetLength(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return 0;

        var length = 0;
        foreach (var c in input)
        {
            var len = Encoding.UTF8.GetByteCount(new[] { c });
            if (len > 1)
                len = 2;
            length += len;
        }
        return length;
    }

    /// <summary>
    /// Encode
    /// </summary>
    /// <param name="input"></param>
    /// <param name="encodeMode"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static string Encode(this string input, EncodeType encodeMode, Encoding? encoding = null)
        => (encoding ?? Encoding.UTF8).GetBytes(input).Encode(encodeMode).ToString(true);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static string Base64Encode(this string input, Encoding? encoding = null)
        => Convert.ToBase64String((encoding ?? Encoding.UTF8).GetBytes(input));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static string Base64Decode(this string input, Encoding? encoding = null)
        => (encoding ?? Encoding.UTF8).GetString(Convert.FromBase64String(input));

    /// <summary>
    /// Aes加密
    /// </summary>
    /// <param name="input"></param>
    /// <param name="key">长度32字符</param>
    /// <param name="iv">长度16字符，为空时使用ECB模式，非空时使用CBC模式</param>
    /// <returns>Base64String</returns>
    public static string AesEncrypt(this string input,
        string key,
        string? iv = null)
        => Convert.ToBase64String(Encoding.UTF8.GetBytes(input).AesEncrypt(key, iv));

    /// <summary>
    /// Aes解密
    /// </summary>
    /// <param name="base64String"></param>
    /// <param name="key">长度32字符</param>
    /// <param name="iv">长度16字符，为空时使用ECB模式，非空时使用CBC模式</param>
    /// <returns></returns>
    public static string AesDecrypt(this string base64String,
        string key,
        string? iv = null)
        => Encoding.UTF8.GetString(Convert.FromBase64String(base64String).AesDecrypt(key, iv));

    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="input"></param>
    /// <param name="iv">长度8字符</param>
    /// <param name="key">长度24字符</param>
    /// <returns>Base64String</returns>
    public static string TripleDESEncrypt(this string input,
        string key,
        string? iv = null)
        => Convert.ToBase64String(Encoding.UTF8.GetBytes(input).TripleDESEncrypt(key, iv));

    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="base64String"></param>
    /// <param name="iv">长度8字符</param>
    /// <param name="key">长度24字符</param>
    /// <returns></returns>
    public static string TripleDESDecrypt(this string base64String,
        string key,
        string? iv = null)
        => Encoding.UTF8.GetString(Convert.FromBase64String(base64String).TripleDESDecrypt(key, iv));

    /// <summary>
    /// 签名
    /// </summary>
    /// <param name="input"></param>
    /// <param name="privateKey_xml">私钥</param>
    /// <param name="hashAlgorithmName"></param>
    /// <param name="rsaSignaturePadding"></param>
    /// <returns>Base64String</returns>
    public static string RSASign(this string input,
        Stream privateKey_xml,
        HashAlgorithmName hashAlgorithmName,
        RSASignaturePadding? rsaSignaturePadding = default)
        => Convert.ToBase64String(Encoding.UTF8.GetBytes(input).RSASign(privateKey_xml, hashAlgorithmName, rsaSignaturePadding));

    /// <summary>
    /// 验签
    /// </summary>
    /// <param name="input"></param>
    /// <param name="signature"></param>
    /// <param name="publicKey_xml"></param>
    /// <param name="hashAlgorithmName"></param>
    /// <param name="rsaSignaturePadding"></param>
    /// <returns></returns>
    public static bool RSAVerify(this string input,
        string signature,
        Stream publicKey_xml,
        HashAlgorithmName hashAlgorithmName,
        RSASignaturePadding? rsaSignaturePadding = default)
        => Encoding.UTF8.GetBytes(input).RSAVerify(Convert.FromBase64String(signature), publicKey_xml, hashAlgorithmName, rsaSignaturePadding);

    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="input"></param>
    /// <param name="publicKey_xml">公钥</param>
    /// <param name="encryptionPadding">OaepSHA1|OaepSHA256|OaepSHA384|OaepSHA512|Pkcs1</param>
    /// <returns>Base64String</returns>
    public static string RSAEncrypt(this string input,
        Stream publicKey_xml,
        RSAEncryptionPadding? encryptionPadding = default)
        => Convert.ToBase64String(Encoding.UTF8.GetBytes(input).RSAEncrypt(publicKey_xml, encryptionPadding));

    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="base64String"></param>
    /// <param name="privateKey_xml">私钥</param>
    /// <param name="encryptionPadding"></param>
    /// <returns></returns>
    public static string RSADecrypt(this string base64String,
        Stream privateKey_xml,
        RSAEncryptionPadding? encryptionPadding = default)
        => Encoding.UTF8.GetString(Convert.FromBase64String(base64String).RSADecrypt(privateKey_xml, encryptionPadding));

    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="input"></param>
    /// <param name="publicKey_str">公钥</param>
    /// <param name="encryptionPadding"></param>
    /// <returns>Base64String</returns>
    public static string RSAEncrypt(this string input,
        string publicKey_str,
        RSAEncryptionPadding? encryptionPadding = default)
        => Convert.ToBase64String(Encoding.UTF8.GetBytes(input).RSAEncrypt(publicKey_str, encryptionPadding));

    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="base64String"></param>
    /// <param name="privateKey_str">私钥</param>
    /// <param name="encryptionPadding"></param>
    /// <returns></returns>
    public static string RSADecrypt(this string base64String,
        string privateKey_str,
        RSAEncryptionPadding? encryptionPadding = default)
        => Encoding.UTF8.GetString(Convert.FromBase64String(base64String).RSADecrypt(privateKey_str, encryptionPadding));

    /// <summary>
    /// 签名
    /// </summary>
    /// <param name="input"></param>
    /// <param name="privateKey_str">私钥</param>
    /// <param name="hashAlgorithmName"></param>
    /// <param name="rsaSignaturePadding"></param>
    /// <returns>Base64String</returns>
    public static string RSASign(this string input,
        string privateKey_str,
        HashAlgorithmName hashAlgorithmName,
        RSASignaturePadding? rsaSignaturePadding = default)
        => Convert.ToBase64String(Encoding.UTF8.GetBytes(input).RSASign(privateKey_str, hashAlgorithmName, rsaSignaturePadding));

    /// <summary>
    /// 验签
    /// </summary>
    /// <param name="input"></param>
    /// <param name="signature"></param>
    /// <param name="publicKey_str"></param>
    /// <param name="hashAlgorithmName"></param>
    /// <param name="rsaSignaturePadding"></param>
    /// <returns></returns>
    public static bool RSAVerify(this string input,
        string signature,
        string publicKey_str,
        HashAlgorithmName hashAlgorithmName,
        RSASignaturePadding? rsaSignaturePadding = default)
        => Encoding.UTF8.GetBytes(input).RSAVerify(Convert.FromBase64String(signature), publicKey_str, hashAlgorithmName, rsaSignaturePadding);

    /// <summary>
    /// N进制字符串转int64
    /// </summary>
    /// <param name="input"></param>
    /// <param name="radix"></param>
    /// <param name="radixCodes"></param>
    /// <returns></returns>
    public static long GetLong(this string input, int radix = 62, string radixCodes = RadixHelper.RadixCodes)
        => RadixHelper.GetLong(input, radix, radixCodes);

    /// <summary>
    /// 转换为骆驼命名法
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string ToCamelCase(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        if (input.Length <= 2)
            return input.ToLower();

        var i = 0;
        var find = false;

        for (; i < input.Length; i++)
        {
            var value = (int)input[i];
            if (value < 65 || value > 90)
            {
                find = true;
                break;
            }
        }

        if (!find)
            i++;

        if (i < 1)
            return input;
        else if (i == 1)
            return $"{input[..1].ToLower()}{input[1..]}";
        else
            return $"{input[..(i - 1)].ToLower()}{input[(i - 1)..]}";
    }

    /// <summary>
    /// 获取HTML文档(部分文档)中的文本内容
    /// </summary>
    /// <param name="input">HTML文档(部分文档)</param>
    /// <returns></returns>
    public static string ClearHTML(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        var output = input;
        output = Regex.Replace(output, @"(<script[^>]*>[.\n]*?</script>)+?", string.Empty, RegexOptions.IgnoreCase);
        output = Regex.Replace(output, @"(<[^>]*>)+?", string.Empty, RegexOptions.IgnoreCase);
        output = Regex.Replace(output, @"([\r\n])[\s]+", string.Empty, RegexOptions.IgnoreCase);
        output = Regex.Replace(output, @"-->", string.Empty, RegexOptions.IgnoreCase);
        output = Regex.Replace(output, @"<!--.*", string.Empty, RegexOptions.IgnoreCase);
        output = Regex.Replace(output, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
        output = Regex.Replace(output, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
        output = Regex.Replace(output, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
        output = Regex.Replace(output, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
        output = Regex.Replace(output, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
        output = Regex.Replace(output, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
        output = Regex.Replace(output, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
        output = Regex.Replace(output, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
        output = Regex.Replace(output, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
        output = Regex.Replace(output, @"&#(\d+);", string.Empty, RegexOptions.IgnoreCase);
        output.Replace("<", string.Empty);
        output.Replace(">", string.Empty);
        output.Replace(Environment.NewLine, string.Empty);
        return output;
    }

    /// <summary>
    /// 截取当前字符串的指定长度，并附加一个指定的结尾
    /// 如果指定长度大于当前字符串长度，则返回当前字符串本身
    /// 如果该字符串为null，则返回0长度字符串
    /// </summary>
    /// <param name="input"></param>
    /// <param name="startIndex">截取开始位置</param>
    /// <param name="length">截取后的字符串长度</param>
    /// <param name="endWith">为结果字符串附加一个结尾</param>
    /// <returns></returns>
    public static string Substring(this string input, int startIndex, int length, string endWith)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        if (input.Length <= length)
            return input;

        return $"{input[startIndex..(startIndex + length)]}{endWith}";
    }

    /// <summary>
    /// 获取指定长度的随机字符串
    /// </summary>
    /// <param name="input"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string Random(this string input, int? length = null)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        var chars = input
            .ToCharArray()
            .OrderBy(_ => Guid.NewGuid())
            .Take(length ?? new Random(Guid.NewGuid().GetHashCode()).Next(input.Length + 1));
        return string.Join(string.Empty, chars);
    }

    /// <summary>
    /// 指示所指定的正则表达式在指定的输入字符串中是否找到了匹配项。
    /// </summary>
    /// <param name="input"></param>
    /// <param name="pattern"></param>
    /// <returns></returns>
    public static bool IsMatch(this string input, string pattern)
        => Regex.IsMatch(input, pattern);

    /// <summary>
    /// 指示所指定的正则表达式在指定的输入字符串中是否找到了匹配项。
    /// </summary>
    /// <param name="input"></param>
    /// <param name="pattern"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static bool IsMatch(this string input, string pattern, RegexOptions options)
        => Regex.IsMatch(input, pattern, options);

    /// <summary>
    /// 指示所指定的正则表达式在指定的输入字符串中是否找到了匹配项。
    /// </summary>
    /// <param name="input"></param>
    /// <param name="pattern"></param>
    /// <param name="options"></param>
    /// <param name="matchTimeout"></param>
    /// <returns></returns>
    public static bool IsMatch(this string input, string pattern, RegexOptions options, TimeSpan matchTimeout)
        => Regex.IsMatch(input, pattern, options, matchTimeout);

    /// <summary>
    /// 在指定的输入字符串中搜索指定的正则表达式的第一个匹配项。
    /// </summary>
    /// <param name="input"></param>
    /// <param name="pattern"></param>
    /// <returns></returns>
    public static Match Match(this string input, string pattern)
        => Regex.Match(input, pattern);

    /// <summary>
    /// 使用指定的匹配选项在输入字符串中搜索指定的正则表达式的第一个匹配项。
    /// </summary>
    /// <param name="input"></param>
    /// <param name="pattern"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static Match Match(this string input, string pattern, RegexOptions options)
        => Regex.Match(input, pattern, options);

    /// <summary>
    /// 使用指定的匹配选项和超时间隔在输入字符串中搜索指定的正则表达式的第一个匹配项。
    /// </summary>
    /// <param name="input"></param>
    /// <param name="pattern"></param>
    /// <param name="options"></param>
    /// <param name="matchTimeout"></param>
    /// <returns></returns>
    public static Match Match(this string input, string pattern, RegexOptions options, TimeSpan matchTimeout)
        => Regex.Match(input, pattern, options, matchTimeout);

    /// <summary>
    /// 在指定的输入字符串中搜索指定的正则表达式的所有匹配项。
    /// </summary>
    /// <param name="input"></param>
    /// <param name="pattern"></param>
    /// <returns></returns>
    public static MatchCollection Matches(this string input, string pattern)
        => Regex.Matches(input, pattern);

    /// <summary>
    /// 使用指定的匹配选项在指定的输入字符串中搜索指定的正则表达式的所有匹配项。
    /// </summary>
    /// <param name="input"></param>
    /// <param name="pattern"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static MatchCollection Matches(this string input, string pattern, RegexOptions options)
        => Regex.Matches(input, pattern, options);

    /// <summary>
    /// 使用指定的匹配选项和超时间隔在指定的输入字符串中搜索指定的正则表达式的所有匹配项。
    /// </summary>
    /// <param name="input"></param>
    /// <param name="pattern"></param>
    /// <param name="options"></param>
    /// <param name="matchTimeout"></param>
    /// <returns></returns>
    public static MatchCollection Matches(this string input, string pattern, RegexOptions options, TimeSpan matchTimeout)
        => Regex.Matches(input, pattern, options, matchTimeout);

    /// <summary>
    /// 在指定的输入字符串内，使用指定的替换字符串替换与指定正则表达式匹配的所有字符串。指定的选项将修改匹配操作。
    /// </summary>
    /// <param name="input"></param>
    /// <param name="pattern"></param>
    /// <param name="replacement"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static string Replace(this string input, string pattern, string replacement, RegexOptions options)
        => Regex.Replace(input, pattern, replacement, options);

    /// <summary>
    /// 在指定的输入字符串内，使用指定的替换字符串替换与指定正则表达式匹配的所有字符串。如果未找到匹配项，则其他参数指定修改匹配操作的选项和超时间隔。
    /// </summary>
    /// <param name="input"></param>
    /// <param name="pattern"></param>
    /// <param name="replacement"></param>
    /// <param name="options"></param>
    /// <param name="matchTimeout"></param>
    /// <returns></returns>
    public static string Replace(this string input, string pattern, string replacement, RegexOptions options, TimeSpan matchTimeout)
        => Regex.Replace(input, pattern, replacement, options, matchTimeout);

    /// <summary>
    /// 在指定的输入字符串内，使用 System.Text.RegularExpressions.MatchEvaluator 委托返回的字符串替换与指定正则表达式匹配的所有字符串。
    /// </summary>
    /// <param name="input"></param>
    /// <param name="pattern"></param>
    /// <param name="evaluator"></param>
    /// <returns></returns>
    public static string Replace(this string input, string pattern, MatchEvaluator evaluator)
        => Regex.Replace(input, pattern, evaluator);

    /// <summary>
    /// 在指定的输入字符串内，使用 System.Text.RegularExpressions.MatchEvaluator 委托返回的字符串替换与指定正则表达式匹配的所有字符串。指定的选项将修改匹配操作。
    /// </summary>
    /// <param name="input"></param>
    /// <param name="pattern"></param>
    /// <param name="evaluator"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static string Replace(this string input, string pattern, MatchEvaluator evaluator, RegexOptions options)
        => Regex.Replace(input, pattern, evaluator, options);

    /// <summary>
    /// 在指定的输入字符串内，使用 System.Text.RegularExpressions.MatchEvaluator 委托返回的字符串替换与指定正则表达式匹配的所有字符串。如果未找到匹配项，则其他参数指定修改匹配操作的选项和超时间隔。
    /// </summary>
    /// <param name="input"></param>
    /// <param name="pattern"></param>
    /// <param name="evaluator"></param>
    /// <param name="options"></param>
    /// <param name="matchTimeout"></param>
    /// <returns></returns>
    public static string Replace(this string input, string pattern, MatchEvaluator evaluator, RegexOptions options, TimeSpan matchTimeout)
        => Regex.Replace(input, pattern, evaluator, options, matchTimeout);

    /// <summary>
    /// 在由指定正则表达式模式定义的位置将输入字符串拆分为一个子字符串数组。指定的选项将修改匹配操作。
    /// </summary>
    /// <param name="input"></param>
    /// <param name="pattern"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static string[] Split(this string input, string pattern, RegexOptions options)
        => Regex.Split(input, pattern, options);

    /// <summary>
    /// 在由指定正则表达式模式定义的位置将输入字符串拆分为一个子字符串数组。如果未找到匹配项，则其他参数指定修改匹配操作的选项和超时间隔。
    /// </summary>
    /// <param name="input"></param>
    /// <param name="pattern"></param>
    /// <param name="options"></param>
    /// <param name="matchTimeout"></param>
    /// <returns></returns>
    public static string[] Split(this string input, string pattern, RegexOptions options, TimeSpan matchTimeout)
        => Regex.Split(input, pattern, options, matchTimeout);

    /// <summary>
    /// 通过替换为转义码来转义最小的字符集（\、*、+、?、|、{、[、(、)、^、$、.、# 和空白）。这将指示正则表达式引擎按原义解释这些字符而不是解释为元字符。
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string Escape(this string input)
        => Regex.Escape(input);

    /// <summary>
    /// 转换输入字符串中的任何转义字符。
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string Unescape(this string input)
        => Regex.Unescape(input);
}
