namespace Liyanjie.ValueObjects;

#if NET6_0_OR_GREATER

public class TimeRange : ValueObject
{
    public TimeOnly Begin { get; set; }

    public TimeOnly End { get; set; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Begin;
        yield return End;
    }
}

#endif