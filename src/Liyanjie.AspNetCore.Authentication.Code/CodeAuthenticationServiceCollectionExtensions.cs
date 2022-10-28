namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 
/// </summary>
public static class CodeAuthenticationServiceCollectionExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="validCode"></param>
    /// <returns></returns>
    public static AuthenticationBuilder AddCode(this AuthenticationBuilder builder, string validCode)
    {
        builder.AddScheme<CodeAuthenticationOptions, CodeAuthenticationHandler>(CodeAuthenticationDefaults.AuthenticationScheme, options =>
        {
            options.ValidCode = validCode;
        });

        return builder;
    }
}
