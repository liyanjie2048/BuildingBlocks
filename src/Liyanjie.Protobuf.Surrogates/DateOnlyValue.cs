namespace Liyanjie.Protobuf.Surrogates;

#if NET6_0_OR_GREATER

[ProtoContract]
public class DateOnlyValue
{
    [ProtoMember(1)] public int DayNumber { get; set; }

    public static implicit operator DateOnlyValue(DateOnly value) => new() { DayNumber = value.DayNumber };
    public static implicit operator DateOnly(DateOnlyValue value) => DateOnly.FromDayNumber(value.DayNumber);
}

#endif