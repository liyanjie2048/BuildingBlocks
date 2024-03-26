using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

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
        if (context.Metadata.IsEnumerableType)
        {
            var delimitedArrayAttribute = ((DefaultModelMetadata)context.Metadata).Attributes.ParameterAttributes?.FirstOrDefault(_ => _ is DelimitedArrayAttribute);
            if (delimitedArrayAttribute is DelimitedArrayAttribute delimitedArray)
            {
                return new DelimitedArrayModelBinder(delimitedArray.Delimiter, delimitedArray.SplitOptions);
            }
        }

        return default;
    }
}
