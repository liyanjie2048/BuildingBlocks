namespace Liyanjie.GrpcServer;

/// <summary>
/// 
/// </summary>
public class GrpcServerHostedService : IHostedService
{
    readonly Grpc.Core.Server server;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="options"></param>
    public GrpcServerHostedService(
        IServiceProvider serviceProvider,
        IOptions<GrpcServerOptions> options)
    {
        this.server = options.Value.CreateServer(serviceProvider);
    }

    /// <inheritdoc/>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        server.Start();
        await Task.CompletedTask;
    }

    /// <inheritdoc/>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await server.ShutdownAsync();
    }
}
