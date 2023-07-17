namespace System.Drawing;

/// <summary>
/// 
/// </summary>
public class ImageX
{
    /// <summary>
    /// 
    /// </summary>
    public readonly static Regex Regex_ImageDataUrl = new(@"^data\:(?<MIME>image\/(bmp|emf|exif|gif|icon|jpeg|png|tiff|wmf))\;base64\,(?<DATA>.+)");

    /// <summary>
    /// 从base64字符串中获取图片
    /// </summary>
    /// <param name="imageBase64String"></param>
    /// <returns></returns>
    public static Image? FromBase64String(string imageBase64String)
    {
        if (string.IsNullOrWhiteSpace(imageBase64String))
            return default;

        try
        {
            return Image.FromStream(new MemoryStream(Convert.FromBase64String(imageBase64String)));
        }
        catch (Exception) { }

        return default;
    }

    public static Image? FromDataUrl(string imageDataUrl)
    {
        if (string.IsNullOrWhiteSpace(imageDataUrl))
            return default;

        var match = Regex_ImageDataUrl.Match(imageDataUrl);
        if (!match.Success)
            return default;

        return FromBase64String(match.Groups["DATA"].Value);
    }
}
