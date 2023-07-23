namespace Liyanjie.GrpcServer;

/// <summary>
/// 
/// </summary>
public class GrpcServerHostedService : IHostedService
{
    readonly Grpc.Core.Server _server;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="options"></param>
    public GrpcServerHostedService(
        IServiceProvider serviceProvider,
        IOptions<GrpcServerOptions> options)
    {
        this._server = options.Value.CreateServer(serviceProvider);
    }

    /// <inheritdoc/>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _server.Start();
        await Task.CompletedTask;
    }

    /// <inheritdoc/>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _server.ShutdownAsync();
    }
}
