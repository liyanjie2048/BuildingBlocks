namespace Liyanjie.ValueObjects;

public class Range<TValue> : ValueObject
    where TValue : struct, IEquatable<TValue>
{
    public TValue From { get; set; }

    public TValue To { get; set; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return From;
        yield return To;
    }
}
