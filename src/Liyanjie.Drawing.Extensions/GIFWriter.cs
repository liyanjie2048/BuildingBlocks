using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace System.Drawing
{
    public class GifWriter : IDisposable
    {
        const long SourceGlobalColorInfoPosition = 10;
        const long SourceImageBlockPosition = 789;

        readonly BinaryWriter writer;
        readonly object syncLock = new();

        bool firstFrame = true;

        public GifWriter(
            Stream outStream,
            int defaultFrameDelay = 500,
            int repeat = 0)
        {
            if (outStream == null)
                throw new ArgumentNullException(nameof(outStream));

            if (defaultFrameDelay <= 0)
                throw new ArgumentOutOfRangeException(nameof(defaultFrameDelay));

            if (repeat < -1)
                repeat = -1;

            this.writer = new BinaryWriter(outStream);
            this.DefaultFrameDelay = defaultFrameDelay;
            this.Repeat = repeat;
        }

        public int DefaultWidth { get; set; }
        public int DefaultHeight { get; set; }
        public int DefaultFrameDelay { get; set; }
        public int Repeat { get; }

        public void Dispose()
        {
            writer.Write((byte)0x3b);
            writer.BaseStream.Dispose();
            writer.Dispose();
        }

        public void WriteFrame(
            Image image,
            int delay = 0)
        {
            lock (syncLock)
            {
                using var gifStream = new MemoryStream();
                image.Save(gifStream, ImageFormat.Gif);

                if (firstFrame)
                    InitHeader(gifStream, writer, image.Width, image.Height);

                WriteGraphicControlBlock(gifStream, writer, delay == 0 ? DefaultFrameDelay : delay);
                WriteImageBlock(gifStream, writer, !firstFrame, 0, 0, image.Width, image.Height);
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

            sourceGif.Position = SourceGlobalColorInfoPosition;
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
            int frameDelay)
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
            sourceGif.Position = SourceImageBlockPosition;

            var header = new byte[11];
            sourceGif.Read(header, 0, header.Length);
            writer.Write(header[0]);
            writer.Write((short)x);
            writer.Write((short)y);
            writer.Write((short)width);
            writer.Write((short)height);

            if (includeColorTable)
            {
                sourceGif.Position = SourceGlobalColorInfoPosition;
                writer.Write((byte)(sourceGif.ReadByte() & 0x3f | 0x80));
                WriteColorTable(sourceGif, writer);
            }
            else
                writer.Write((byte)(header[9] & 0x07 | 0x07));

            writer.Write(header[10]);

            sourceGif.Position = SourceImageBlockPosition + header.Length;

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
}
