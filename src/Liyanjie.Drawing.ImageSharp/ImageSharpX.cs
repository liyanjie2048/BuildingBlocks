namespace Liyanjie.Drawing.ImageSharp;

public partial class ImageSharpX
{
#if NET8_0_OR_GREATER
    [GeneratedRegex(@"^data\:(?<MIME>image\/(bmp|emf|exif|gif|icon|jpeg|png|tiff|wmf))\;base64\,(?<DATA>.+)")]
    private static partial Regex _Regex_ImageDataUrl();
#endif

    /// <summary>
    /// 
    /// </summary>
    public readonly static Regex Regex_ImageDataUrl =
#if NET8_0_OR_GREATER
        _Regex_ImageDataUrl();
#else
        new(@"^data\:(?<MIME>image\/(bmp|emf|exif|gif|icon|jpeg|png|tiff|wmf))\;base64\,(?<DATA>.+)");
#endif

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
            return Image.Load(Convert.FromBase64String(imageBase64String));
        }
        catch (Exception) { }

        return default;
    }

    /// <summary>
    /// 从base64字符串中获取图片
    /// </summary>
    /// <param name="imageBase64String"></param>
    /// <returns></returns>
    public static Image<TPixel>? FromBase64String<TPixel>(string imageBase64String)
        where TPixel : unmanaged, IPixel<TPixel>
    {
        if (string.IsNullOrWhiteSpace(imageBase64String))
            return default;

        try
        {
            return Image.Load<TPixel>(Convert.FromBase64String(imageBase64String));
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

    public static Image<TPixel>? FromDataUrl<TPixel>(string imageDataUrl)
        where TPixel : unmanaged, IPixel<TPixel>
    {
        if (string.IsNullOrWhiteSpace(imageDataUrl))
            return default;

        var match = Regex_ImageDataUrl.Match(imageDataUrl);
        if (!match.Success)
            return default;

        return FromBase64String<TPixel>(match.Groups["DATA"].Value);
    }

    public static Image Load(string path) => Image.Load(path);
    public static Image Load(DecoderOptions options, string path) => Image.Load(options, path);
    public static Task<Image> LoadAsync(string path, CancellationToken cancellationToken = default) => Image.LoadAsync(path, cancellationToken);
    public static Task<Image> LoadAsync(DecoderOptions options, string path, CancellationToken cancellationToken = default) => Image.LoadAsync(options, path, cancellationToken);
    public static Image<TPixel> Load<TPixel>(string path) where TPixel : unmanaged, IPixel<TPixel> => Image.Load<TPixel>(path);
    public static Image<TPixel> Load<TPixel>(DecoderOptions options, string path) where TPixel : unmanaged, IPixel<TPixel> => Image.Load<TPixel>(options, path);
    public static Task<Image<TPixel>> LoadAsync<TPixel>(string path, CancellationToken cancellationToken = default) where TPixel : unmanaged, IPixel<TPixel> => Image.LoadAsync<TPixel>(path, cancellationToken);
    public static Task<Image<TPixel>> LoadAsync<TPixel>(DecoderOptions options, string path, CancellationToken cancellationToken = default) where TPixel : unmanaged, IPixel<TPixel> => Image.LoadAsync<TPixel>(options, path, cancellationToken);
}
