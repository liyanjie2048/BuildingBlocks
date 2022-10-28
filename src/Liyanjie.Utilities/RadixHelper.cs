namespace System;

/// <summary>
/// 
/// </summary>
public static class RadixHelper
{
    /// <summary>
    /// 将 long 型数字转换为 x 进制字符串
    /// </summary>
    /// <param name="number"></param>
    /// <param name="radix"></param>
    /// <param name="radixCodes"></param>
    /// <returns></returns>
    public static string ToString(long number, int radix = 62, string radixCodes = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz")
    {
        var result = string.Empty;
        var fu = false;

        if (number < 0)
        {
            number = -number;
            fu = true;
        }

        var list = new List<long>();
        long tmp;

        while (number > radix)
        {
            tmp = number % radix;
            number -= tmp;
            number /= radix;

            if (string.IsNullOrWhiteSpace(radixCodes))
            {
                tmp += 48;
                if (tmp > 57)
                    tmp += 7;
                if (tmp > 90)
                    tmp += 6;
            }

            list.Add(tmp);
        }
        tmp = number % radix;

        if (string.IsNullOrWhiteSpace(radixCodes))
        {
            tmp += 48;
            if (tmp > 57)
                tmp += 7;
            if (tmp > 90)
                tmp += 6;
        }

        list.Add(tmp);

        foreach (var item in list)
        {
            result = (string.IsNullOrWhiteSpace(radixCodes) ? ((char)item).ToString() : radixCodes[(int)item].ToString()) + result;
        }

        if (fu)
            result = "-" + result;

        return result.ToString();
    }

    /// <summary>
    /// 将 x 进制数字字符串转换为 long 型
    /// </summary>
    /// <param name="input"></param>
    /// <param name="radix">进制</param>
    /// <param name="radixCodes"></param>
    /// <returns></returns>
    public static long GetLong(string input, int radix = 62, string radixCodes = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz")
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException($"Parameter '{nameof(input)}' can't be Null nor Empty nor WhiteSpace", nameof(input));

        var result = 0L;
        var fu = false;

        if (input.IndexOf('-') == 0)
        {
            input = input[1..];
            fu = true;
        }

        if (radix <= 36)
            input = input.ToUpper();

        var array = input.ToArray().Reverse().ToList();

        for (int i = 0; i < array.Count; i++)
        {
            long num;
            if (string.IsNullOrWhiteSpace(radixCodes))
            {
                num = array[i];

                if (num > 96)
                    num -= 6;
                if (num > 64)
                    num -= 7;
                if (num > 47)
                    num -= 48;
            }
            else
            {
                num = radixCodes.IndexOf(array[i]);
                if (num < 0)
                    throw new Exception($"{array[i]} is not in {nameof(radixCodes)}");
            }

            for (int j = 0; j < i; j++)
            {
                num *= radix;
            }

            result += num;
        }

        if (fu)
            result = -result;

        return result;
    }
}
