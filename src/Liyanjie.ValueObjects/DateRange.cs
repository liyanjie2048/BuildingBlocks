namespace Liyanjie.ValueObjects;

#if NET6_0_OR_GREATER

public class DateRange : ValueObject
{
    public DateOnly Begin { get; set; }

    public DateOnly End { get; set; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Begin;
        yield return End;
    }
}

#endif