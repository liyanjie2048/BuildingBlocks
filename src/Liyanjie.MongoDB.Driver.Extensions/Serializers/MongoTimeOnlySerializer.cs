#if NET6_0_OR_GREATER

namespace Liyanjie.MongoDB.Driver.Extensions.Serializers;

public class MongoTimeOnlySerializer : StructSerializerBase<TimeOnly>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, TimeOnly value)
    {
        //年份小于1970会报错
        var dateTime = new DateTime(1970, 1, 1, value.Hour, value.Minute, value.Second, value.Millisecond, DateTimeKind.Local);
        context.Writer.WriteDateTime(BsonUtils.ToMillisecondsSinceEpoch(dateTime.ToUniversalTime()));
    }

    public override TimeOnly Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var dateTime = BsonUtils.ToDateTimeFromMillisecondsSinceEpoch(context.Reader.ReadDateTime()).ToLocalTime();
        return TimeOnly.FromDateTime(dateTime);
    }
}

#endif
