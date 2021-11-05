using System;
using System.Configuration;
using System.IO;
using System.Linq;
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
        static readonly string[] startup;
        static readonly string[] urls;
        static IHost host;
        static CancellationTokenSource cts;

        static HostManager()
        {
            try
            {
                startup = ConfigurationManager.AppSettings["Startup"]?.Split(',', StringSplitOptions.RemoveEmptyEntries);
                urls = ConfigurationManager.AppSettings["Urls"]?.Split(',', StringSplitOptions.RemoveEmptyEntries);
            }
            catch { }
        }

        internal static void Start()
        {
            if (startup is null || startup.Length < 2)
            {
                MessageBox.Show("AppSettings\\Startup 配置错误");
                return;
            }

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
            try
            {
                cts = new();
                host = Host.CreateDefaultBuilder(Environment.GetCommandLineArgs())
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup(Assembly.LoadFrom(startup[0]).GetType(startup[1]));
                        if (urls?.Length > 0)
                            webBuilder.UseUrls(urls);
                        webBuilder.ConfigureLogging(logging => logging.AddProvider(new Logging.MyLoggerProvider()));
                    })
                    .Build();
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
                    host.StopAsync(cts.Token).ConfigureAwait(false);
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
            return urls?.Length > 0 ? urls : new[] { "http://localhost:5000" };
        }
    }
}
