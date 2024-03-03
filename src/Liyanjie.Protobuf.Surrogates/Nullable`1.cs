namespace Liyanjie.Protobuf.Surrogates;

[ProtoContract]
public class Nullable<TValue>
{
    [ProtoMember(1)]
    public TValue? Value { get; set; }

    public static implicit operator Nullable<TValue>(TValue? value) => new() { Value = value };
    public static implicit operator TValue?(Nullable<TValue> value) => value.Value;
}
