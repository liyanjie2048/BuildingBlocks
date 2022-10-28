namespace Liyanjie.TypeBuilder;

/// <summary>
/// 
/// </summary>
public abstract class DynamicBase
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public object GetPropertyValue(string name)
    {
        return GetType().GetTypeInfo().GetProperty(name).GetValue(this);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public void SetPropertyValue(string name, object value)
    {
        GetType().GetTypeInfo().GetProperty(name).SetValue(this, value);
    }
}
