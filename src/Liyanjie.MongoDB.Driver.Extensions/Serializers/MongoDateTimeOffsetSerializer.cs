#if NET6_0_OR_GREATER

namespace Liyanjie.MongoDB.Driver.Extensions.Serializers;

public class MongoDateTimeOffsetSerializer : StructSerializerBase<DateTimeOffset>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateTimeOffset value)
    {
        context.Writer.WriteDateTime(BsonUtils.ToMillisecondsSinceEpoch(value.UtcDateTime));
    }

    public override DateTimeOffset Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        return new DateTimeOffset(BsonUtils.ToDateTimeFromMillisecondsSinceEpoch(context.Reader.ReadDateTime())).ToLocalTime();
    }
}

#endif
