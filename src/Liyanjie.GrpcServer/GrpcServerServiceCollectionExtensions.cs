namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 
/// </summary>
public static class GrpcServerServiceCollectionExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configureOptions"></param>
    /// <returns></returns>
    public static IServiceCollection AddGrpcServer(this IServiceCollection services,
        Action<GrpcServerOptions> configureOptions)
    {
        services.Configure(configureOptions);
        services.AddHostedService<GrpcServerHostedService>();

        return services;
    }
}
