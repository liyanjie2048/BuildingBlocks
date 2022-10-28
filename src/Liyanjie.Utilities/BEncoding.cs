namespace Liyanjie.Utilities;

public class BEncoding
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static object? Decode(Stream stream, Encoding encoding)
    {
        using var reader = new BinaryReader(stream, encoding);
        var bytesConsumed = 0;
        return Decode(reader, ref bytesConsumed);
    }
    static object? Decode(BinaryReader reader, ref int bytesConsumed)
    {
        var next = (char)reader.PeekChar();

        return next switch
        {
            'i' => DecodeInteger(reader, ref bytesConsumed),
            'l' => DecodeList(reader, ref bytesConsumed),
            'd' => DecodeDictionary(reader, ref bytesConsumed),
            '0' or '1' or '2' or '3' or '4' or '5' or '6' or '7' or '8' or '9' => DecodeBytes(reader, ref bytesConsumed),// String
            _ => null,
        };
    }
    static Dictionary<string, object?> DecodeDictionary(BinaryReader reader, ref int bytesConsumed)
    {
        // Get past 'd'
        reader.ReadByte();

        bytesConsumed++;
        var result = new Dictionary<string, object?>();

        // Read elements till an 'e'
        while (reader.PeekChar() != 'e')
        {
            var key = Encoding.ASCII.GetString(DecodeBytes(reader, ref bytesConsumed));// Key
            var value = Decode(reader, ref bytesConsumed);// Value

            result[key] = value;
        }

        // Get past 'e'
        reader.Read();
        bytesConsumed++;

        return result;
    }
    static List<object?> DecodeList(BinaryReader reader, ref int bytesConsumed)
    {
        // Get past 'l'
        reader.Read();
        bytesConsumed++;

        var list = new List<object?>();

        // Read elements till an 'e'
        while (reader.PeekChar() != 'e')
        {
            list.Add(Decode(reader, ref bytesConsumed));
        }

        // Get past 'e'
        reader.Read();
        bytesConsumed++;

        return list;
    }
    static byte[] DecodeBytes(BinaryReader reader, ref int bytesConsumed)
    {
        // Read up to ':'
        var numberLength = string.Empty;
        char ch;

        while ((ch = reader.ReadChar()) != ':')
        {
            numberLength += ch;
            bytesConsumed++;
        }

        bytesConsumed++;

        // Read chars out
        var data = reader.ReadBytes(int.Parse(numberLength));
        bytesConsumed += int.Parse(numberLength);

        return data;
    }
    static long DecodeInteger(BinaryReader reader, ref int bytesConsumed)
    {
        // Get past 'i'
        reader.Read();
        bytesConsumed++;

        // Read numbers till an 'e'
        var number = string.Empty;
        char ch;

        while ((ch = reader.ReadChar()) != 'e')
        {
            number += ch;
            bytesConsumed++;
        }

        bytesConsumed++;

        return long.Parse(number);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="encoding"></param>
    /// <param name="data"></param>
    public static void Encode(Stream stream, Encoding encoding, object data)
    {
        using var writer = new BinaryWriter(stream, encoding);
        Encode(writer, data);
        writer.Flush();
    }
    static void Encode(BinaryWriter writer, object data)
    {
        if (data is Dictionary<string, object> dictionary)
            EncodeDictionary(writer, dictionary);
        else if (data is List<object> list)
            EncodeList(writer, list);
        else if (data is byte[] bytes)
            EncodeBytes(writer, bytes);
        else if (data is long integer)
            EncodeInteger(writer, integer);
    }
    static void EncodeDictionary(BinaryWriter writer, Dictionary<string, object> dictionary)
    {
        // Write header
        writer.Write('d');

        // Write elements
        foreach (var item in dictionary)
        {
            EncodeBytes(writer, Encoding.ASCII.GetBytes(item.Key));
            Encode(writer, item.Value);
        }

        // Write footer
        writer.Write('e');
    }
    static void EncodeList(BinaryWriter writer, List<object> list)
    {
        // Write header
        writer.Write('l');

        // Write elements
        foreach (var item in list)
        {
            Encode(writer, item);
        }

        // Write footer
        writer.Write('e');
    }
    static void EncodeBytes(BinaryWriter writer, byte[] bytes)
    {
        // Write length
        writer.Write(Encoding.ASCII.GetBytes(bytes.Length.ToString()));

        // Write seperator
        writer.Write(':');

        // Write ASCII representation
        writer.Write(bytes);
    }
    static void EncodeInteger(BinaryWriter writer, long integer)
    {
        // Write header
        writer.Write('i');

        // Write value
        writer.Write(integer.ToString().ToCharArray());

        // Write footer
        writer.Write('e');
    }
}
