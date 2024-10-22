using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pollr.AdminUI.Models;

namespace Pollr.AdminUI
{
    public class Startup
    {
        private readonly ILogger<Startup> _logger;
        private readonly IConfiguration _config;


        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            _config = configuration;
            _logger = logger;
        }

        //public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Enable IOptions in the DI container
            services.AddOptions();

            // Indicate how ClientConfiguration options should be constructed
            services.Configure<ClientConfiguration>(_config.GetSection("ClientConfiguration"));
            var value = _config["ClientConfiguration:HubUrl"];

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration => {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IConfiguration config)
        {
            loggerFactory
                .AddConsole()
                .AddDebug()
                .AddAzureWebAppDiagnostics();

            _logger.LogInformation("### Environment: {0}", env.EnvironmentName);
            _logger.LogInformation("### ApiUrl: {0}", config["ClientConfiguration:ApiUrl"]);
            _logger.LogInformation("### HubUrl: {0}", config["ClientConfiguration:HubUrl"]);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa => {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment()) {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
