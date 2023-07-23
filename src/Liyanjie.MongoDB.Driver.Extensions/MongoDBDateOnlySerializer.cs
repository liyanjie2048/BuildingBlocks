namespace MongoDB.Bson.Serialization.Serializers;

public class MongoDBDateOnlySerializer : StructSerializerBase<DateOnly>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateOnly value)
    {
        var dateDime = value.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        context.Writer.WriteDateTime(BsonUtils.ToMillisecondsSinceEpoch(dateDime));
    }

    public override DateOnly Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var dateTime = BsonUtils.ToDateTimeFromMillisecondsSinceEpoch(context.Reader.ReadDateTime()).ToLocalTime();
        return DateOnly.FromDateTime(dateTime);
    }
}
