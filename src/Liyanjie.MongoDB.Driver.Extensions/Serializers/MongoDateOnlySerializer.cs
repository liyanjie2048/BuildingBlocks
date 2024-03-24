#if NET6_0_OR_GREATER

namespace Liyanjie.MongoDB.Driver.Extensions.Serializers;

public class MongoDateOnlySerializer : StructSerializerBase<DateOnly>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateOnly value)
    {
        var dateTime = value.ToDateTime(TimeOnly.MinValue, DateTimeKind.Local);
        context.Writer.WriteDateTime(BsonUtils.ToMillisecondsSinceEpoch(dateTime.ToUniversalTime()));
    }

    public override DateOnly Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var dateTime = BsonUtils.ToDateTimeFromMillisecondsSinceEpoch(context.Reader.ReadDateTime()).ToLocalTime();
        return DateOnly.FromDateTime(dateTime);
    }
}

#endif