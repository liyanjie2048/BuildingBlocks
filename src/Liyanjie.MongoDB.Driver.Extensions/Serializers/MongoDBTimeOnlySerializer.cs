namespace Liyanjie.MongoDB.Driver.Extensions.Serializers;

public class MongoDBTimeOnlySerializer : StructSerializerBase<TimeOnly>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, TimeOnly value)
    {
        var dateTime = new DateTime(0, 1, 1, value.Hour, value.Minute, value.Second, value.Millisecond, DateTimeKind.Local);
        context.Writer.WriteDateTime(BsonUtils.ToMillisecondsSinceEpoch(dateTime.ToUniversalTime()));
    }

    public override TimeOnly Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var dateTime = BsonUtils.ToDateTimeFromMillisecondsSinceEpoch(context.Reader.ReadDateTime()).ToLocalTime();
        return TimeOnly.FromDateTime(dateTime);
    }
}
