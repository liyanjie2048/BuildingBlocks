using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Liyanjie.AspNetCore.Extensions;

/// <summary>
/// [ModelBinder(BinderType = typeof(DelimitedArrayModelBinder))]
/// public string[] ModelProperty { get; set; }
/// </summary>
public class DelimitedArrayModelBinder : IModelBinder
{
    readonly string _delimiter;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="delimiter"></param>
    public DelimitedArrayModelBinder(string delimiter = ",")
    {
        this._delimiter = delimiter;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindingContext"></param>
    /// <returns></returns>
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext is null)
            throw new ArgumentNullException(nameof(bindingContext));

        if (!bindingContext.ModelMetadata.IsEnumerableType)
            return;

        var values = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
        if (values == ValueProviderResult.None)
            return;

        bindingContext.ModelState.SetModelValue(bindingContext.ModelName, values);

        var elementType = bindingContext.ModelMetadata.ElementType;
        if (elementType is null)
            return;
        var converter = TypeDescriptor.GetConverter(elementType);

        try
        {
            var value = values
                .SelectMany(_ => _.Split(new[] { _delimiter }, StringSplitOptions.RemoveEmptyEntries)
                .Select(_ => converter.ConvertFromString(_)))
                .ToArray();
            var typedValue = Array.CreateInstance(elementType, value.Length);
            value.CopyTo(typedValue, 0);
            bindingContext.Result = ModelBindingResult.Success(typedValue);
        }
        catch (Exception e)
        {
            bindingContext.ModelState.AddModelError(bindingContext.ModelName, e.Message);
        }

        await Task.CompletedTask;
    }
}
