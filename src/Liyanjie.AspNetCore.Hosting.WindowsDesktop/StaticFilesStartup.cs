using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Liyanjie.AspNetCore.Hosting.WindowsDesktop
{
    public class StaticFilesStartup
    {
        public StaticFilesStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) { }

        public void Configure(IApplicationBuilder app)
        {
            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.Map("/", async context =>
                {
                    await Task.CompletedTask;
                    context.Response.Redirect("/index.html");
                });
            });
        }
    }
}
