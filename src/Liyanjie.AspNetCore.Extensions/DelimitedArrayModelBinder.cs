namespace Liyanjie.AspNetCore.Extensions;

/// <summary>
/// 只能用在Query或Form绑定数据时使用
/// 
/// [ModelBinder(BinderType = typeof(DelimitedArrayModelBinder))]
/// public string[] ModelProperty { get; set; }
/// </summary>
/// <param name="delimiter"></param>
public class DelimitedArrayModelBinder(string delimiter = ",") : IModelBinder
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindingContext"></param>
    /// <returns></returns>
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        await Task.CompletedTask;

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
                .SelectMany(_ => _.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries)
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
    }
}
