namespace Liyanjie.MongoDB.Driver.Extensions.Serializers;

public class MongoDBDateTimeOffsetSerializer : StructSerializerBase<DateTimeOffset>
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
