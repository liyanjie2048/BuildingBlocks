namespace Liyanjie.Drawing.SkiaSharp;

public static class SKEncodedImageFormatExtensions
{
    public static string GetMIMEType(this SKEncodedImageFormat imageFormat)
    {
        return imageFormat switch
        {
            SKEncodedImageFormat.Bmp => "image/bmp",
            SKEncodedImageFormat.Gif => "image/gif",
            SKEncodedImageFormat.Ico => "image/icon",
            SKEncodedImageFormat.Jpeg => "image/jpeg",
            SKEncodedImageFormat.Png => "image/png",
            SKEncodedImageFormat.Wbmp => "image/vnd.wap.wbmp",
            SKEncodedImageFormat.Webp => "image/webp",
            //SKEncodedImageFormat.Pkm => "",
            //SKEncodedImageFormat.Ktx => "",
            //SKEncodedImageFormat.Astc => "",
            //SKEncodedImageFormat.Dng => "",
            //SKEncodedImageFormat.Heif => "",
            SKEncodedImageFormat.Avif => "image/avif",
            _ => "application/octet-stream",
        };
    }
}
