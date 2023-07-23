namespace MongoDB.Bson.Serialization.Serializers;

public class MongoDBTimeOnlySerializer : StructSerializerBase<TimeOnly>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, TimeOnly value)
    {
        var dateTime = new DateTime(2000, 1, 1, value.Hour, value.Minute, value.Second, value.Millisecond);
        context.Writer.WriteDateTime(BsonUtils.ToMillisecondsSinceEpoch(dateTime));
    }

    public override TimeOnly Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var dateTime = BsonUtils.ToDateTimeFromMillisecondsSinceEpoch(context.Reader.ReadDateTime()).ToLocalTime();
        return TimeOnly.FromDateTime(dateTime);
    }
}
