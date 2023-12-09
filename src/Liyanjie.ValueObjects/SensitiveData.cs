﻿namespace Liyanjie.ValueObjects;

/// <summary>
/// 敏感数据
/// </summary>
public class SensitiveData : ValueObject
{
    public string? Origin { get; set; }
    public string? Modification { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool? Status { get; set; }

    public void Modify(string? modification)
    {
        Modification = modification;
        Status = null;
    }

    public void Audit(bool? status)
    {
        Status = status;
        if (true == status)
            Origin = Modification;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Origin;
        yield return Modification;
    }
}
