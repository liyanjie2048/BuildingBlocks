namespace Liyanjie.EnglishPluralization.Internals;

internal static class Check
{
    public static T NotNull<T>(T value, string parameterName) where T : class
    {
        if (value is null)
        {
            throw new ArgumentNullException(parameterName);
        }
        return value;
    }

    public static T? NotNull<T>(T? value, string parameterName) where T : struct
    {
        if (!value.HasValue)
        {
            throw new ArgumentNullException(parameterName);
        }
        return value;
    }

    public static string NotEmpty(string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("ֵ����Ϊnull����ַ�����", "parameterName");
        }
        return value;
    }
}
