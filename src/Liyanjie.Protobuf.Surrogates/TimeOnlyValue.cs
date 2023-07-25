namespace Liyanjie.Protobuf.Surrogates;

#if NET6_0_OR_GREATER

[ProtoContract]
public class TimeOnlyValue
{
    [ProtoMember(1)] public long Ticks { get; set; }

    public static implicit operator TimeOnlyValue(TimeOnly value) => new() { Ticks = value.Ticks };
    public static implicit operator TimeOnly(TimeOnlyValue value) => new(value.Ticks);
}

#endif