namespace System.Drawing.Imaging;

public static class ImageFormatExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="imageFormat"></param>
    /// <returns></returns>
    public static string GetMIMEType(this ImageFormat imageFormat)
    {
        return imageFormat switch
        {
            var x when ImageFormat.Bmp.Equals(x) => "image/bmp",
            var x when ImageFormat.Emf.Equals(x) => "image/emf",
            var x when ImageFormat.Exif.Equals(x) => "image/exif",
            var x when ImageFormat.Gif.Equals(x) => "image/gif",
            var x when ImageFormat.Icon.Equals(x) => "image/icon",
            var x when ImageFormat.Jpeg.Equals(x) => "image/jpeg",
            var x when ImageFormat.Png.Equals(x) => "image/png",
            var x when ImageFormat.Tiff.Equals(x) => "image/tiff",
            var x when ImageFormat.Wmf.Equals(x) => "image/wmf",
            _ => "application/octet-stream",
        };
    }
}
