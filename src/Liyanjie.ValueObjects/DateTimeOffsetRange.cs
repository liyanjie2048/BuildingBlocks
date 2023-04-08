namespace Liyanjie.ValueObjects;

public class DateTimeOffsetRange : ValueObject
{
    public DateTimeOffset Begin { get; set; }

    public DateTimeOffset End { get; set; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Begin;
        yield return End;
    }
}
