namespace SkiaSharp;

/// <summary>
/// 
/// </summary>
public static class SKImageExtensions
{
    /// <summary>
    /// 将图片转码为base64字符串
    /// </summary>
    /// <param name="image"></param>
    /// <param name="format"></param>
    /// <returns></returns>
    public static string ToBase64String(this SKImage image,
        (SKEncodedImageFormat Format, int Quality)? format = default)
    {
        if (image is null)
            throw new ArgumentNullException(nameof(image));

        var (Format, Quality) = format ?? (SKEncodedImageFormat.Png, 100);
        return Convert.ToBase64String(image.Encode(Format, Quality).ToArray());
    }

    public static string ToDataUrl(this SKImage image,
        (SKEncodedImageFormat Format, int Quality)? format = default)
    {
        if (image is null)
            throw new ArgumentNullException(nameof(image));

        var format_ = format ?? (SKEncodedImageFormat.Png, 100);
        return $"data:{format_.Format.GetMIMEType()};base64,{ToBase64String(image, format_)}";
    }

    /// <summary>
    /// 改变透明度
    /// </summary>
    /// <param name="image"></param>
    /// <param name="opacity"></param>
    public static SKImage SetOpacity(this SKImage image, float opacity)
    {
        if (image is null)
            throw new ArgumentNullException(nameof(image));

        if (opacity < 0 || opacity > 1)
            throw new ArgumentOutOfRangeException(nameof(opacity), "不透明度必须为0~1之间的浮点数");

        using var bitmap = SKBitmap.FromImage(image);
        using var output = new SKBitmap(image.Width, image.Height, SKColorType.Rgba8888, SKAlphaType.Unpremul);
        for (int x = 0; x < image.Width; x++)
        {
            for (int y = 0; y < image.Height; y++)
            {
                output.SetPixel(x, y, bitmap.GetPixel(x, y).WithAlpha((byte)(0xFF * opacity)));
            }
        }

        return SKImage.FromBitmap(output);
    }

    /// <summary>
    /// 清除整个图像并以指定颜色填充
    /// </summary>
    /// <param name="image"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public static SKImage Clear(this SKImage image, SKColor color)
    {
        if (image is null)
            throw new ArgumentNullException(nameof(image));

        using var bitmap = SKBitmap.FromImage(image);
        for (int x = 0; x < image.Width; x++)
        {
            for (int y = 0; y < image.Height; y++)
            {
                bitmap.SetPixel(x, y, color);
            }
        }

        return SKImage.FromBitmap(bitmap);
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
    public static SKImage Crop(this SKImage image,
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

        return Crop(image, new(startX, startY, startX + width, startY + height));
    }

    /// <summary>
    /// 裁剪图片
    /// </summary>
    /// <param name="image"></param>
    /// <param name="rect"></param>
    /// <returns></returns>
    public static SKImage Crop(this SKImage image, SKRect rect)
    {
        if (image is null)
            throw new ArgumentNullException(nameof(image));

        using var bitmap = new SKBitmap((int)rect.Width, (int)rect.Height);
        using var canvas = new SKCanvas(bitmap);
        using var source = SKBitmap.FromImage(image);
        canvas.DrawBitmap(source,
            new SKRect(rect.Left, rect.Top, rect.Right, rect.Bottom),
            new SKRect(0, 0, rect.Width, rect.Height));

        return SKImage.FromBitmap(bitmap);
    }

    /// <summary>
    /// 调整大小
    /// </summary>
    /// <param name="image"></param>
    /// <param name="width">宽</param>
    /// <param name="height">高</param>
    /// <param name="quality">质量</param>
    /// <param name="zoom">等比缩放</param>
    /// <param name="cover">Ture：在同时指定宽和高并且等比缩放的情况下，将裁剪图片以满足宽高比</param>
    /// <returns></returns>
    public static SKImage Resize(this SKImage image,
        int? width,
        int? height,
        SKFilterQuality quality = SKFilterQuality.None,
        bool zoom = true,
        bool cover = false)
    {
        if (image is null)
            throw new ArgumentNullException(nameof(image));

        width = width > 0 ? width : null;
        height = height > 0 ? height : null;

        int w, h;
        if (width.HasValue && height.HasValue)
        {
            var wRatio = (double)image.Width / width.Value;
            var hRatio = (double)image.Height / height.Value;
            if (zoom)
            {
                if (cover)
                {
                    w = width.Value;
                    h = height.Value;
                    if (wRatio > hRatio)
                    {
                        w = (int)(w * hRatio);
                        h = image.Height;
                    }
                    else
                    {
                        w = image.Width;
                        h = (int)(h * wRatio);
                    }
                }
                else
                {
                    if (wRatio > hRatio)
                    {
                        w = width.Value;
                        h = (int)(image.Height / wRatio);
                    }
                    else
                    {
                        w = (int)(image.Width / hRatio);
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
        else
        {
            return image;
        }

        using var bitmap = SKBitmap.FromImage(image);
        return SKImage.FromBitmap(bitmap.Resize(new SKSizeI(w, h), quality));
    }

    /// <summary>
    /// 组合多张图片
    /// </summary>
    /// <param name="image">底图</param>
    /// <param name="image2">图片</param>
    /// <param name="point">位置</param>
    /// <param name="size">大小</param>
    /// <returns></returns>
    public static SKImage Combine(this SKImage image,
        SKImage image2,
        SKPoint point,
        SKSize size)
    {
        if (image is null)
            throw new ArgumentNullException(nameof(image));

        if (image2 is null)
            return image;

        using var bitmap = SKBitmap.FromImage(image);
        using var canvas = new SKCanvas(bitmap);
        using var source = SKBitmap.FromImage(image2);
        canvas.DrawBitmap(source, SKRect.Create(point, size));

        return SKImage.FromBitmap(bitmap);
    }

    /// <summary>
    /// 拼接图片
    /// </summary>
    /// <param name="image"></param>
    /// <param name="image2"></param>
    /// <param name="direction">true=水平方向，false=垂直方向</param>
    /// <returns></returns>
    public static SKImage Concatenate(this SKImage image,
         SKImage image2,
        bool direction = false)
    {
        if (image is null)
            throw new ArgumentNullException(nameof(image));

        if (image2 is null)
            return image;

        if (direction)
        {
            var output = new SKBitmap(image.Width + image2.Width, Math.Max(image.Height, image2.Height));
            using var canvas = new SKCanvas(output);

            using var bitmap = SKBitmap.FromImage(image);
            canvas.DrawBitmap(bitmap, 0, 0);

            using var bitmap2 = SKBitmap.FromImage(image2);
            canvas.DrawBitmap(bitmap2, image.Width, 0);

            return SKImage.FromBitmap(output);
        }
        else
        {
            var output = new SKBitmap(Math.Max(image.Width, image2.Width), image.Height + image2.Height);
            using var canvas = new SKCanvas(output);

            using var bitmap = SKBitmap.FromImage(image);
            canvas.DrawBitmap(bitmap, 0, 0);

            using var bitmap2 = SKBitmap.FromImage(image2);
            canvas.DrawBitmap(bitmap2, 0, image.Height);

            return SKImage.FromBitmap(output);
        }
    }

    /// <summary>
    /// 存储
    /// </summary>
    /// <param name="image"></param>
    /// <param name="path"></param>
    /// <param name="format"></param>
    public static void Save(this SKImage image,
        string path,
        (SKEncodedImageFormat Format, int Quality)? format = default)
    {
        var (Format, Quality) = format ?? (SKEncodedImageFormat.Png, 100);
        File.WriteAllBytes(path, image.Encode(Format, Quality).ToArray());
    }

    /// <summary>
    /// 存储
    /// </summary>
    /// <param name="image"></param>
    /// <param name="path"></param>
    /// <param name="format"></param>
    public static async Task SaveAsync(this SKImage image,
        string path,
        (SKEncodedImageFormat Format, int Quality)? format = default)
    {
        var (Format, Quality) = format ?? (SKEncodedImageFormat.Png, 100);
        await File.WriteAllBytesAsync(path, image.Encode(Format, Quality).ToArray());
    }
}
