namespace Liyanjie.ValueObjects;

/// <summary>
/// 敏感数据
/// </summary>
public class Sensitive<TData> : ValueObject
{
    public TData? Origin { get; set; }
    public TData? Modification { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool? Status { get; set; } = false;

    public void Modify(TData? modification)
    {
        if ((Origin is null && modification is null)
            || Origin?.Equals(modification) == true)
            return;

        Modification = modification;
        Status = null;
    }

    public void Audit(bool? status)
    {
        Status = status;
        if (true == status)
        {
            Origin = Modification;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Origin;
        yield return Modification;
        yield return Status;
    }
}
public class Sensitive : Sensitive<string> { }
