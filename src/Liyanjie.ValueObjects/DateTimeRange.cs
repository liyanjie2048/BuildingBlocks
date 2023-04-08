namespace Liyanjie.ValueObjects;

public class DateTimeRange : ValueObject
{
    public DateTime Begin { get; set; }

    public DateTime End { get; set; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Begin;
        yield return End;
    }
}
