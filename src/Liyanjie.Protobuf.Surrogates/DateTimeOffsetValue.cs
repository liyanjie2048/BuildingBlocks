namespace Liyanjie.Protobuf.Surrogates;

[ProtoContract]
public class DateTimeOffsetValue
{
    [ProtoMember(1)] public long Ticks { get; set; }
    [ProtoMember(2)] public TimeSpan Offset { get; set; }

    public static implicit operator DateTimeOffsetValue(DateTimeOffset value) => new()
    {
        Ticks = value.Ticks,
        Offset = value.Offset,
    };
    public static implicit operator DateTimeOffset(DateTimeOffsetValue value) => new(value.Ticks, value.Offset);
}

