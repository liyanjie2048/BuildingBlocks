namespace System.Drawing;

public class GIFWriter : IDisposable
{
    const long _sourceGlobalColorInfoPosition = 10;
    const long _sourceImageBlockPosition = 789;

    readonly BinaryWriter _writer;
    readonly object _syncLock = new();

    bool firstFrame = true;

    public GIFWriter(
        Stream outStream,
        uint defaultFrameDelay = 500,
        int repeat = 0)
    {
        if (outStream is null)
            throw new ArgumentNullException(nameof(outStream));

        if (defaultFrameDelay <= 0)
            throw new ArgumentOutOfRangeException(nameof(defaultFrameDelay));

        _writer = new BinaryWriter(outStream);
        DefaultFrameDelay = defaultFrameDelay;
        Repeat = repeat < -1 ? -1 : repeat;
    }

    public int DefaultWidth { get; set; }
    public int DefaultHeight { get; set; }
    public uint DefaultFrameDelay { get; set; }
    public int Repeat { get; }

    public void Dispose()
    {
        _writer.Write((byte)0x3b);
        _writer.BaseStream.Dispose();
        _writer.Dispose();
    }

    public void WriteFrame(
        Image image,
        uint delay = 0)
    {
        lock (_syncLock)
        {
            using var gifStream = new MemoryStream();
            image.Save(gifStream, ImageFormat.Gif);

            if (firstFrame)
                InitHeader(gifStream, _writer, image.Width, image.Height);

            WriteGraphicControlBlock(gifStream, _writer, delay == 0 ? DefaultFrameDelay : delay);
            WriteImageBlock(gifStream, _writer, !firstFrame, 0, 0, image.Width, image.Height);
        }

        if (firstFrame)
            firstFrame = false;
    }

    void InitHeader(
        Stream sourceGif,
        BinaryWriter writer,
        int width,
        int height)
    {
        writer.Write("GIF".ToCharArray());
        writer.Write("89a".ToCharArray());

        writer.Write((short)(DefaultWidth == 0 ? width : DefaultWidth));
        writer.Write((short)(DefaultHeight == 0 ? height : DefaultHeight));

        sourceGif.Position = _sourceGlobalColorInfoPosition;
        writer.Write((byte)sourceGif.ReadByte());
        writer.Write((byte)0);
        writer.Write((byte)0);
        WriteColorTable(sourceGif, writer);

        if (Repeat == -1)
            return;

        writer.Write(unchecked((short)0xff21));
        writer.Write((byte)0x0b);
        writer.Write("NETSCAPE2.0".ToCharArray());
        writer.Write((byte)3);
        writer.Write((byte)1);
        writer.Write((short)Repeat);
        writer.Write((byte)0);
    }

    static void WriteColorTable(
        Stream sourceGif,
        BinaryWriter writer)
    {
        sourceGif.Position = 13;
        var colorTable = new byte[768];
        sourceGif.Read(colorTable, 0, colorTable.Length);
        writer.Write(colorTable, 0, colorTable.Length);
    }

    static void WriteGraphicControlBlock(
        Stream sourceGif,
        BinaryWriter writer,
        uint frameDelay)
    {
        sourceGif.Position = 781;
        var blockhead = new byte[8];
        sourceGif.Read(blockhead, 0, blockhead.Length);

        writer.Write(unchecked((short)0xf921));
        writer.Write((byte)0x04);
        writer.Write((byte)(blockhead[3] & 0xf7 | 0x08));
        writer.Write((short)(frameDelay / 10));
        writer.Write(blockhead[6]);
        writer.Write((byte)0);
    }

    static void WriteImageBlock(
        Stream sourceGif,
        BinaryWriter writer,
        bool includeColorTable,
        int x,
        int y,
        int width,
        int height)
    {
        sourceGif.Position = _sourceImageBlockPosition;

        var header = new byte[11];
        sourceGif.Read(header, 0, header.Length);
        writer.Write(header[0]);
        writer.Write((short)x);
        writer.Write((short)y);
        writer.Write((short)width);
        writer.Write((short)height);

        if (includeColorTable)
        {
            sourceGif.Position = _sourceGlobalColorInfoPosition;
            writer.Write((byte)(sourceGif.ReadByte() & 0x3f | 0x80));
            WriteColorTable(sourceGif, writer);
        }
        else
            writer.Write((byte)(header[9] & 0x07 | 0x07));

        writer.Write(header[10]);

        sourceGif.Position = _sourceImageBlockPosition + header.Length;

        var dataLength = sourceGif.ReadByte();
        while (dataLength > 0)
        {
            var imgData = new byte[dataLength];
            sourceGif.Read(imgData, 0, dataLength);

            writer.Write((byte)dataLength);
            writer.Write(imgData, 0, dataLength);
            dataLength = sourceGif.ReadByte();
        }

        writer.Write((byte)0);
    }
}
