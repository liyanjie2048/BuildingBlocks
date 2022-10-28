namespace Liyanjie.GrpcServer;

/// <summary>
/// 
/// </summary>
public class GrpcServerOptions
{
    readonly List<ServerPort> _ports = new();
    readonly List<ServerServiceDefinition> _services = new();
    readonly List<Func<IServiceProvider, ServerServiceDefinition>> _serviceFactories = new();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="host"></param>
    /// <param name="port"></param>
    /// <param name="credentials"></param>
    /// <returns></returns>
    public GrpcServerOptions AddPort(string host, int port, ServerCredentials? credentials = default)
    {
        _ports.Add(new ServerPort(host, port, default == credentials ? ServerCredentials.Insecure : credentials));
        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceDefinition"></param>
    /// <returns></returns>
    public GrpcServerOptions AddService(ServerServiceDefinition serviceDefinition)
    {
        _services.Add(serviceDefinition);
        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceFactory"></param>
    /// <returns></returns>
    public GrpcServerOptions AddService(Func<IServiceProvider, ServerServiceDefinition> serviceFactory)
    {
        _serviceFactories.Add(serviceFactory);
        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Server CreateServer(IServiceProvider serviceProvider)
    {
        var server = new Server();
        foreach (var port in _ports)
        {
            server.Ports.Add(port);
        }
        foreach (var service in _services)
        {
            server.Services.Add(service);
        }
        foreach (var factory in _serviceFactories)
        {
            server.Services.Add(factory.Invoke(serviceProvider));
        }
        return server;
    }
}
