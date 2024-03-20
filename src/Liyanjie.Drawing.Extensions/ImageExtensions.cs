namespace System.Drawing;

/// <summary>
/// 
/// </summary>
public static class ImageExtensions
{
    /// <summary>
    /// 将图片转码为base64字符串
    /// </summary>
    /// <param name="image"></param>
    /// <param name="format"></param>
    /// <returns></returns>
    public static string ToBase64String(this Image image, ImageFormat? format = default)
    {
        if (image is null)
            throw new ArgumentNullException(nameof(image));

        using var memory = new MemoryStream();
        image.Save(memory, format ?? image.RawFormat);
        return Convert.ToBase64String(memory.ToArray());
    }

    public static string ToDataUrl(this Image image, ImageFormat? imageFormat = default)
    {
        if (image is null)
            throw new ArgumentNullException(nameof(image));

        return $"data:{(imageFormat ?? image.RawFormat).GetMIMEType()};base64,{ToBase64String(image, imageFormat)}";
    }

    /// <summary>
    /// 改变透明度
    /// </summary>
    /// <param name="image"></param>
    /// <param name="opacity"></param>
    public static Image SetOpacity(this Image image, float opacity)
    {
        if (image is null)
            throw new ArgumentNullException(nameof(image));

        if (opacity < 0 || opacity > 1)
            throw new ArgumentOutOfRangeException(nameof(opacity), "不透明度必须为0~1之间的浮点数");

        var colorMatrix = new ColorMatrix(
        [
            [1, 0, 0, 0, 0],
            [0, 1, 0, 0, 0],
            [0, 0, 1, 0, 0],
            [0, 0, 0, opacity, 0],
            [0, 0, 0, 0, 1]
        ]);
        var imageAttributes = new ImageAttributes();
        imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

        var bitmap = new Bitmap(image);

        var output = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);
        for (int x = 0; x < image.Width; x++)
        {
            for (int y = 0; y < image.Height; y++)
            {
                var color = bitmap.GetPixel(x, y);
                output.SetPixel(x, y, Color.FromArgb((int)(0xFF * opacity), color.R, color.G, color.B));
            }
        }

        return output;
    }

    /// <summary>
    /// 清除整个图像并以指定颜色填充
    /// </summary>
    /// <param name="image"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public static Image Clear(this Image image, Color color)
    {
        if (image is null)
            throw new ArgumentNullException(nameof(image));

        using var graphics = Graphics.FromImage(image);
        graphics.Clear(color);

        return image;
    }

    /// <summary>
    /// 裁剪图片
    /// </summary>
    /// <param name="image">源图片</param>
    /// <param name="startX">裁剪开始 X 坐标</param>
    /// <param name="startY">裁剪开始 Y 坐标</param>
    /// <param name="width">裁剪宽度</param>
    /// <param name="height">裁剪高度</param>
    /// <returns></returns>
    public static Image Crop(this Image image,
        int startX,
        int startY,
        int width,
        int height)
    {
        if (startX < 0)
            startX = 0;
        if (startY < 0)
            startY = 0;
        if (width > image.Width - startX)
            width = image.Width - startX;
        if (height > image.Height - startY)
            height = image.Height - startY;

        return Crop(image, new Rectangle(startX, startY, width, height));
    }

    /// <summary>
    /// 裁剪图片
    /// </summary>
    /// <param name="image"></param>
    /// <param name="rectangle"></param>
    /// <returns></returns>
    public static Image Crop(this Image image, Rectangle rectangle)
    {
        if (image is null)
            throw new ArgumentNullException(nameof(image));

        if (image.RawFormat.Guid == ImageFormat.Gif.Guid)
        {
            var repeatCount = BitConverter.ToUInt16(image.GetPropertyItem(0x5101)!.Value!, 0);
            var frames = GetGifFrames(image).Select(_ => (Image: _.Image.Crop(rectangle), _.DelayByMilliseconds)).ToArray();

            return frames[0].Image.CombineToGif(frames[0].DelayByMilliseconds, repeatCount, frames[1..]);
        }
        else
        {
            using var output = new Bitmap(image);
            return output.Clone(rectangle, image.PixelFormat);
        }
    }

    /// <summary>
    /// 调整大小
    /// </summary>
    /// <param name="image"></param>
    /// <param name="width">宽</param>
    /// <param name="height">高</param>
    /// <param name="zoom">等比缩放</param>
    /// <param name="coverSize">Ture：在同时指定宽和高并且等比缩放的情况下，将裁剪图片以满足宽高比</param>
    /// <returns></returns>
    public static Image Resize(this Image image,
        int? width,
        int? height,
        bool zoom = true,
        bool coverSize = false)
    {
        if (image is null)
            throw new ArgumentNullException(nameof(image));

        width = width == 0 ? null : width;
        height = height == 0 ? null : height;

        if (width is null && height is null)
            return image;

        var w = image.Width;
        var h = image.Height;

        if (width.HasValue && height.HasValue)
        {
            var wW = (double)image.Width / width.Value;
            var hH = (double)image.Height / height.Value;
            if (zoom)
            {
                if (coverSize)
                {
                    w = width.Value;
                    h = height.Value;
                    if (wW > hH)
                    {
                        var _width = (int)(w * hH);
                        image = image.Crop(Math.Abs(image.Width - _width) / 2, 0, _width, image.Height);
                    }
                    else
                    {
                        var _height = (int)(h * wW);
                        image = image.Crop(0, Math.Abs(image.Height - _height) / 2, image.Width, _height);
                    }
                }
                else
                {
                    if (wW > hH)
                    {
                        w = width.Value;
                        h = (int)(image.Height / wW);
                    }
                    else
                    {
                        w = (int)(image.Width / hH);
                        h = height.Value;
                    }
                }
            }
            else
            {
                w = width.Value;
                h = height.Value;
            }
        }
        else if (width.HasValue)
        {
            w = width.Value;
            h = zoom ? (int)(image.Height / ((double)image.Width / width.Value)) : image.Height;
        }
        else if (height.HasValue)
        {
            w = zoom ? (int)(image.Width / ((double)image.Height / height.Value)) : image.Width;
            h = height.Value;
        }

        if (image.RawFormat.Guid == ImageFormat.Gif.Guid)
        {
            var repeatCount = BitConverter.ToUInt16(image.GetPropertyItem(0x5101)!.Value!, 0);
            var frames = GetGifFrames(image).Select(_ => (Image: _Resize(_.Image, w, h), _.DelayByMilliseconds)).ToArray();

            return frames[0].Image.CombineToGif(frames[0].DelayByMilliseconds, repeatCount, frames[1..]);
        }
        else
        {
            var output = new Bitmap(w, h);
            using var graphics = Graphics.FromImage(output);
            graphics.DrawImage(image, 0, 0, w, h);

            return _Resize(image, w, h);
        }

        static Image _Resize(Image image, int width, int height)
        {
            var output = new Bitmap(width, height);
            using var graphics = Graphics.FromImage(output);
            graphics.DrawImage(image, 0, 0, width, height);

            return output;
        }
    }

    /// <summary>
    /// 组合图片
    /// </summary>
    /// <param name="image">底图</param>
    /// <param name="image2">图片</param>
    /// <param name="point">位置</param>
    /// <param name="size">大小</param>
    /// <returns></returns>
    public static Image Combine(this Image image,
        Image image2,
        Point point,
        Size size)
    {
        if (image is null)
            throw new ArgumentNullException(nameof(image));

        if (image2 is null)
            return image;

        using var graphics = Graphics.FromImage(image);
        graphics.DrawImage(image2, new Rectangle(point, size));

        return image;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="image"></param>
    /// <param name="delayByMilliseconds"></param>
    /// <param name="repeatCount"></param>
    /// <param name="images"></param>
    /// <returns></returns>
    public static Image CombineToGif(this Image image,
        int delayByMilliseconds = 0,
        ushort repeatCount = 0,
        params (Image Image, int DelayByMilliseconds)[] images)
    {
        if (image is null)
            throw new ArgumentNullException(nameof(image));

        if (images is null || images.Length == 0)
            return image;

        using var memory = new MemoryStream();
        using var gif = new GifWriter(memory, repeatCount: repeatCount);
        gif.WriteFrame(image, delayByMilliseconds);

        foreach (var (Image, DelayByMilliseconds) in images)
        {
            gif.WriteFrame(Image, DelayByMilliseconds);
        }

        return Image.FromStream(memory);
    }

    /// <summary>
    /// 拼接图片
    /// </summary>
    /// <param name="image"></param>
    /// <param name="image2"></param>
    /// <param name="direction">true=水平方向，false=垂直方向</param>
    /// <returns></returns>
    public static Image Concatenate(this Image image,
        Image image2,
        bool direction = false)
    {
        if (image is null)
            throw new ArgumentNullException(nameof(image));

        if (image2 is null)
            return image;

        if (direction)
        {
            var output = new Bitmap(image.Width + image2.Width, Math.Max(image.Height, image2.Height));

            using var graphics = Graphics.FromImage(output);
            graphics.DrawImage(image, 0, 0);
            graphics.DrawImage(image2, image.Width, 0);

            return output;
        }
        else
        {
            var output = new Bitmap(Math.Max(image.Width, image2.Width), image.Height + image2.Height);

            using var graphics = Graphics.FromImage(output);
            graphics.DrawImage(image, 0, 0);
            graphics.DrawImage(image2, 0, image.Height);

            return output;
        }
    }

    /// <summary>
    /// 压缩存储
    /// </summary>
    /// <param name="image"></param>
    /// <param name="path"></param>
    /// <param name="quality">质量，0~100</param>
    /// <param name="format"></param>
    public static void CompressSave(this Image image,
        string path,
        int quality,
        ImageFormat? format = default)
    {
        var format_ = format ?? GetFormat(Path.GetExtension(path)) ?? image.RawFormat;
        var imageCodecInfo = ImageCodecInfo.GetImageEncoders().FirstOrDefault(_ => _.FormatID == format_.Guid);
        if (imageCodecInfo is null)
        {
            image.Save(path, format_);
        }
        else
        {
            using var encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, quality < 0 ? 0 : quality > 100 ? 100 : quality);
            image.Save(path, imageCodecInfo, encoderParameters);
        }
    }

    static List<(Image Image, int DelayByMilliseconds)> GetGifFrames(Image image)
    {
        var output = new List<(Image Image, int DelayByMilliseconds)>();

        var property = image.GetPropertyItem(0x5100)!;
        var frameDimension = new FrameDimension(image.FrameDimensionsList[0]);
        for (int i = 0; i < image.GetFrameCount(frameDimension); i++)
        {
            image.SelectActiveFrame(frameDimension, i);

            var bitmap = new Bitmap(image.Width, image.Height);
            var graphics = Graphics.FromImage(bitmap);
            graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height));

            var bytes = new[]
            {
                property.Value![i * 4],
                property.Value![1 + i * 4],
                property.Value![2 + i * 4],
                property.Value![3 + i * 4]
            };
            var delay = BitConverter.ToInt32(bytes, 0) * 10;

            output.Add((bitmap, delay));
        }

        return output;
    }

    static ImageFormat GetFormat(string extension)
    {
        return extension.ToLower() switch
        {
            ".bmp" => ImageFormat.Bmp,
            ".emf" => ImageFormat.Emf,
            ".exif" => ImageFormat.Exif,
            ".gif" => ImageFormat.Gif,
            ".icon" or ".ico" => ImageFormat.Icon,
            ".jpeg" or ".jpg" => ImageFormat.Jpeg,
            ".png" => ImageFormat.Png,
            ".tiff" or ".tif" => ImageFormat.Tiff,
            ".wmf" => ImageFormat.Wmf,
            _ => ImageFormat.Jpeg,
        };
    }
}
