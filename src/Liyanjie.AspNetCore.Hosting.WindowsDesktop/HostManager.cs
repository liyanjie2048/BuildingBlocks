using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Liyanjie.AspNetCore.Hosting.WindowsDesktop
{
    class HostManager
    {
        static readonly string _contentRoot;
        static readonly string _webRoot;
        static readonly Type _startupType;
        static readonly string[] _urls;

        static CancellationTokenSource cts;
        static IHost host;

        static HostManager()
        {
            try
            {
                _contentRoot = ConfigurationManager.AppSettings["ContentRoot"];
                _webRoot = ConfigurationManager.AppSettings["WebRoot"];
                var startup = ConfigurationManager.AppSettings["Startup"]?.Split(',', StringSplitOptions.RemoveEmptyEntries);
                _startupType = Assembly.LoadFrom(startup[0]).GetType(startup[1]);
                _urls = ConfigurationManager.AppSettings["Urls"]?.Split(',', StringSplitOptions.RemoveEmptyEntries);
            }
            catch { }
        }

        internal static void Start()
        {
            foreach (var item in Directory.EnumerateFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll", new EnumerationOptions
            {
                RecurseSubdirectories = true,
            }))
            {
                try
                {
                    Assembly.LoadFrom(item);
                }
                catch { }
            }

            void ConfigureWebHost(IWebHostBuilder webBuilder)
            {
                if (!string.IsNullOrWhiteSpace(_contentRoot))
                    webBuilder.UseWebRoot(_contentRoot);
                if (!string.IsNullOrWhiteSpace(_webRoot))
                    webBuilder.UseWebRoot(_webRoot);
                webBuilder.UseStartup(_startupType);
                if (_urls?.Length > 0)
                    webBuilder.UseUrls(_urls);
                webBuilder.ConfigureLogging(logging => logging.AddProvider(new Logging.MyLoggerProvider()));
            }

            try
            {
                cts = new();
#if NET6_0_OR_GREATER
                var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder();
                ConfigureWebHost(builder.WebHost);
                host=builder.Build();
#else
                host = Host.CreateDefaultBuilder()
                    .ConfigureWebHostDefaults(ConfigureWebHost)
                    .Build();
#endif
                host.RunAsync(cts.Token);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                cts?.Cancel();
            }
        }
        internal static void Stop()
        {
            try
            {
                if (host is not null)
                {
                    host.StopAsync().ConfigureAwait(false);
                    host.Dispose();
                }
            }
            catch
            {
                cts?.Cancel();
            }
        }
        internal static string[] GetUrls()
        {
            return _urls?.Length > 0 ? _urls : new[] { "http://localhost:5000" };
        }
    }
}
