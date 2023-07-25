namespace ProtoBuf.Meta;

public static class Extensions
{
    public static void AddDateTimeOffsetSurrogate(this RuntimeTypeModel model)
    {
        model.Add(typeof(DateTimeOffset), true).SetSurrogate(typeof(DateTimeOffsetValue));
    }

#if NET6_0_OR_GREATER

    public static void AddDateOnlySurrogate(this RuntimeTypeModel model)
    {
        model.Add(typeof(DateOnly), true).SetSurrogate(typeof(DateOnlyValue));
    }

    public static void AddTimeOnlySurrogate(this RuntimeTypeModel model)
    {
        model.Add(typeof(TimeOnly), true).SetSurrogate(typeof(TimeOnlyValue));
    }

#endif
}
