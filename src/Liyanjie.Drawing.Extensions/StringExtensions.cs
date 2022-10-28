namespace System;

/// <summary>
/// 
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// 从base64字符串中获取图片
    /// </summary>
    /// <param name="imageBase64String"></param>
    /// <returns></returns>
    public static Bitmap Decode(this string imageBase64String)
    {
        return new Bitmap(new MemoryStream(Convert.FromBase64String(imageBase64String)));
    }
}
