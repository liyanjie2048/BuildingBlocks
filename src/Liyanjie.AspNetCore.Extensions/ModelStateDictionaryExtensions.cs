using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Microsoft.AspNetCore.Mvc;

/// <summary>
/// 
/// </summary>
public static class ModelStateDictionaryExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="modelState"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static bool IsValid(this ModelStateDictionary modelState, string key)
    {
        if (modelState.ContainsKey(key))
            return modelState[key]!.ValidationState == ModelValidationState.Valid;

        return true;
    }
}
