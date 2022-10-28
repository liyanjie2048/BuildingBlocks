namespace Liyanjie.AspNetCore.Extensions;

/// <summary>
/// services.AddMvc(options => 
/// { 
///     options.ModelBinderProviders.Insert(0, new DelimitedArrayModelBinderProvider());
/// });
/// </summary>
public class DelimitedArrayModelBinderProvider : IModelBinderProvider
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context is null)
            throw new ArgumentNullException(nameof(context));

        if (!context.Metadata.IsEnumerableType)
            return default;

        var propertyName = context.Metadata.PropertyName;
        if (string.IsNullOrEmpty(propertyName))
            return default;

        var propertyAttribute = context.Metadata.ContainerType?.GetProperty(propertyName)?.GetCustomAttributes<DelimitedArrayAttribute>(false).FirstOrDefault();
        if (propertyAttribute is null)
            return default;

        return new DelimitedArrayModelBinder(propertyAttribute.Delimiter);
    }
}
