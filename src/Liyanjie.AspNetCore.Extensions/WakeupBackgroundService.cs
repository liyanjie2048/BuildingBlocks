using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.Hosting;

public class WakeupBackgroundService : BackgroundService
{
    readonly ILogger<WakeupBackgroundService> _logger;
    readonly IHttpClientFactory _httpClientFactory;
    readonly string _wakeupUrl;
    public WakeupBackgroundService(
        ILogger<WakeupBackgroundService> logger,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _wakeupUrl = configuration.GetValue<string?>("WakeupUrl", default) ?? throw new ApplicationException("找不到 WakeupUrl 配置项");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (string.IsNullOrEmpty(_wakeupUrl))
            return;

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);

            _logger.LogTrace("Wake up!");
            try
            {
                using var http = _httpClientFactory.CreateClient();
                var response = await http.GetStringAsync(_wakeupUrl, stoppingToken);

                _logger.LogTrace(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
