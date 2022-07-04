using System.Collections.Generic;

namespace Liyanjie.ValueObjects;

public class Job<TIndustry> : ValueObject
{
    public TIndustry? Industry { get; set; }

    public string? Company { get; set; }

    public string? Position { get; set; }

    public Address? Address { get; set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Industry!;
        yield return Company!;
        yield return Position!;
        yield return Address!;
    }

    public override string ToString() => $"{Company} {Position}";
}
public class Job : Job<string> { }
