namespace Liyanjie.Drawing.SkiaSharp;

public class SKImageX
{
    /// <summary>
    /// 
    /// </summary>
    public readonly static Regex Regex_ImageDataUrl = new(@"^data\:(?<MIME>image\/(bmp|emf|exif|gif|icon|jpeg|png|tiff|wmf))\;base64\,(?<DATA>.+)");

    /// <summary>
    /// 从文件路径读取图片
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static SKImage FromFile(string filePath)
    {
        var bytes = File.ReadAllBytes(filePath);
        return SKImage.FromEncodedData(bytes);
    }

    /// <summary>
    /// 从base64字符串中获取图片
    /// </summary>
    /// <param name="imageBase64String"></param>
    /// <returns></returns>
    public static SKImage? FromBase64String(string imageBase64String)
    {
        if (string.IsNullOrWhiteSpace(imageBase64String))
            return default;

        try
        {
            return SKImage.FromEncodedData(Convert.FromBase64String(imageBase64String));
        }
        catch (Exception) { }

        return default;
    }

    public static SKImage? FromDataUrl(string imageDataUrl)
    {
        if (string.IsNullOrWhiteSpace(imageDataUrl))
            return default;

        var match = Regex_ImageDataUrl.Match(imageDataUrl);
        if (!match.Success)
            return default;

        return FromBase64String(match.Groups["DATA"].Value);
    }
}
