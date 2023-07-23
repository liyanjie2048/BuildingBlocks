namespace System.Drawing;

public class GifWriter : IDisposable
{
    const long _sourceGlobalColorInfoPosition = 10;
    const long _sourceImageBlockPosition = 789;

    readonly BinaryWriter _writer;
    readonly object _syncLock = new();

    bool _firstFrame = true;

    public GifWriter(
        Stream output,
        int defaultFrameDelay = 500,
        ushort repeatCount = 0)
    {
        if (output is null)
            throw new ArgumentNullException(nameof(output));

        if (defaultFrameDelay <= 0)
            throw new ArgumentOutOfRangeException(nameof(defaultFrameDelay));

        _writer = new BinaryWriter(output);
        DefaultFrameDelay = defaultFrameDelay;
        RepeatCount = repeatCount;
    }

    public int DefaultFrameDelay { get; set; }
    public ushort RepeatCount { get; }

    public void Dispose()
    {
        _writer.Write((byte)0x3b);
        _writer.BaseStream.Dispose();
        _writer.Dispose();
    }

    public void WriteFrame(
        Image image,
        int delay = 0)
    {
        lock (_syncLock)
        {
            using var stream = new MemoryStream();
            image.Save(stream, ImageFormat.Gif);

            if (_firstFrame)
                InitHeader(stream, image.Width, image.Height);

            WriteGraphicControlBlock(stream, delay == 0 ? DefaultFrameDelay : delay);
            WriteImageBlock(stream, !_firstFrame, 0, 0, image.Width, image.Height);
        }

        if (_firstFrame)
            _firstFrame = false;
    }

    void InitHeader(
        Stream stream,
        int width,
        int height)
    {
        _writer.Write("GIF".ToCharArray());
        _writer.Write("89a".ToCharArray());

        _writer.Write((short)width);
        _writer.Write((short)height);

        stream.Position = _sourceGlobalColorInfoPosition;
        _writer.Write((byte)stream.ReadByte());
        _writer.Write((byte)0);
        _writer.Write((byte)0);
        WriteColorTable(stream);

        _writer.Write(unchecked((short)0xff21));
        _writer.Write((byte)0x0b);
        _writer.Write("NETSCAPE2.0".ToCharArray());
        _writer.Write((byte)3);
        _writer.Write((byte)1);
        _writer.Write((short)RepeatCount);
        _writer.Write((byte)0);
    }

    void WriteColorTable(
       Stream stream)
    {
        stream.Position = 13;
        var colorTable = new byte[768];
        stream.Read(colorTable, 0, colorTable.Length);
        _writer.Write(colorTable, 0, colorTable.Length);
    }

    void WriteGraphicControlBlock(
       Stream stream,
       int frameDelay)
    {
        stream.Position = 781;
        var blockhead = new byte[8];
        stream.Read(blockhead, 0, blockhead.Length);

        _writer.Write(unchecked((short)0xf921));
        _writer.Write((byte)0x04);
        _writer.Write((byte)(blockhead[3] & 0xf7 | 0x08));
        _writer.Write((short)(frameDelay / 10));
        _writer.Write(blockhead[6]);
        _writer.Write((byte)0);
    }

    void WriteImageBlock(
        Stream stream,
        bool includeColorTable,
        int x,
        int y,
        int width,
        int height)
    {
        stream.Position = _sourceImageBlockPosition;

        var header = new byte[11];
        stream.Read(header, 0, header.Length);
        _writer.Write(header[0]);
        _writer.Write((short)x);
        _writer.Write((short)y);
        _writer.Write((short)width);
        _writer.Write((short)height);

        if (includeColorTable)
        {
            stream.Position = _sourceGlobalColorInfoPosition;
            _writer.Write((byte)(stream.ReadByte() & 0x3f | 0x80));
            WriteColorTable(stream);
        }
        else
            _writer.Write((byte)(header[9] & 0x07 | 0x07));

        _writer.Write(header[10]);

        stream.Position = _sourceImageBlockPosition + header.Length;

        var dataLength = stream.ReadByte();
        while (dataLength > 0)
        {
            var imgData = new byte[dataLength];
            stream.Read(imgData, 0, dataLength);

            _writer.Write((byte)dataLength);
            _writer.Write(imgData, 0, dataLength);
            dataLength = stream.ReadByte();
        }

        _writer.Write((byte)0);
    }
}
