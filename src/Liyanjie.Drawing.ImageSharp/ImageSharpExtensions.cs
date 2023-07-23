namespace Liyanjie.Drawing.ImageSharp;

/// <summary>
/// 
/// </summary>
public static class ImageSharpExtensions
{

    /// <summary>
    /// 将图片转码为base64字符串
    /// </summary>
    /// <param name="image"></param>
    /// <param name="format"></param>
    /// <returns></returns>
    public static string ToBase64String(this Image image,
        IImageFormat? format = default)
    {
        if (image is null)
            throw new ArgumentNullException(nameof(image));

        using var memory = new MemoryStream();
        image.Save(memory, format ?? image.Metadata.DecodedImageFormat ?? JpegFormat.Instance);

        var bytes = memory.ToArray();
        return Convert.ToBase64String(bytes);
    }

    public static string ToDataUrl(this Image image,
        IImageFormat? format = default)
    {
        if (image is null)
            throw new ArgumentNullException(nameof(image));

        var format_ = format ?? image.Metadata.DecodedImageFormat ?? JpegFormat.Instance;
        return $"data:{format_.MimeTypes};base64,{ToBase64String(image, format_)}";
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

        var output = image.CloneAs<Rgba32>();
        output.Mutate(_ => _.Opacity(opacity));

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

        var output = image.CloneAs<Rgba32>();
        output.ProcessPixelRows(_ =>
        {
            for (int y = 0; y < _.Height; y++)
            {
                var pixelRow = _.GetRowSpan(y);
                for (int x = 0; x < pixelRow.Length; x++)
                {
                    ref var pixel = ref pixelRow[x];
                    pixel.FromRgba32(color);
                }
            }
        });

        return output;
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

        return Crop(image, new(startX, startY, width, height));
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

        image.Mutate(_ => _.Crop(rectangle));

        return image;
    }

    /// <summary>
    /// 调整大小
    /// </summary>
    /// <param name="image"></param>
    /// <param name="width">宽</param>
    /// <param name="height">高</param>
    /// <param name="zoom">等比缩放</param>
    /// <param name="cover">Ture：在同时指定宽和高并且等比缩放的情况下，将裁剪图片以满足宽高比</param>
    /// <returns></returns>
    public static Image Resize(this Image image,
        int? width,
        int? height,
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

        image.Mutate(_ => _.Resize(w, h));

        return image;
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

        image2.Mutate(_ => _.Resize(size));
        image.Mutate(_ => _.DrawImage(image2, point, 1));

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

        var gif = image.CloneAs<Rgba32>();
        gif.Metadata.GetGifMetadata().RepeatCount = repeatCount;
        gif.Frames.RootFrame.Metadata.GetGifMetadata().FrameDelay = delayByMilliseconds / 10;

        foreach (var (Image, DelayByMilliseconds) in images)
        {
            var _image = Image.CloneAs<Rgba32>();
            _image.Frames.RootFrame.Metadata.GetGifMetadata().FrameDelay = DelayByMilliseconds / 10;
            gif.Frames.AddFrame(_image.Frames.RootFrame);
        }

        return gif;
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
            var output = new Image<Rgba32>(image.Width + image2.Width, Math.Max(image.Height, image2.Height));
            output.Mutate(_ => _.DrawImage(image, new Point(0, 0), 1));
            output.Mutate(_ => _.DrawImage(image2, new Point(image.Width, 0), 1));

            return output;
        }
        else
        {
            var output = new Image<Rgba32>(Math.Max(image.Width, image2.Width), image.Height + image2.Height);
            output.Mutate(_ => _.DrawImage(image, new Point(0, 0), 1));
            output.Mutate(_ => _.DrawImage(image2, new Point(0, image.Height), 1));

            return output;
        }
    }
}
