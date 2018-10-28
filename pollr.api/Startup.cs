/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pollr.Api;
using Pollr.Api.Dal;
using Pollr.Api.Hubs;

namespace pollr.api
{
    public class Startup
    {
        private readonly ILogger<Startup> _logger;
        bool useAzureSignalRManagedHub = false;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("AllowLocalhost", builder => {
                builder.AllowAnyOrigin()
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            }));

            //services.AddAuthentication(AzureADDefaults.BearerAuthenticationScheme)
            //    .AddAzureADBearer(options => Configuration.Bind("AzureAd", options));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.Configure<DatabaseSettings>(options =>
            {
                options.ConnectionString = Configuration.GetSection("MongoConnection:ConnectionString").Value;
                options.Database = Configuration.GetSection("MongoConnection:Database").Value;
            });

            // Add a SignalR hub:
            // In production we will typically use an Azure Managed hub, but in development
            // we''l just create a local hub
            useAzureSignalRManagedHub = bool.Parse(Configuration.GetSection("SignalR:UseAzureSignalRManagedHub").Value);
            if (useAzureSignalRManagedHub) {
                _logger.LogInformation("Using Azure Managed SignaR hub.");
                services.AddSignalR()
                        .AddAzureSignalR(Configuration.GetSection("SignalR:Azure:SignalR:ConnectionString").Value);
            }
            else {
                _logger.LogInformation("Using local SignaR hub.");
                services.AddSignalR();
            }

            services.AddTransient<IPollDefinitionRepository, PollDefinitionRepository>();
            services.AddTransient<IPollRepository, PollRepository>();

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            _logger.LogInformation($"Environment: {0}", env.EnvironmentName);
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseHsts();
            }

            // Enable Global Cors: Don't do this is a real production app!
            app.UseCors("AllowLocalhost");

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();

            // Configure Signalr
            if (useAzureSignalRManagedHub) {
                app.UseAzureSignalR(routes => {
                    routes.MapHub<VoteHub>("/voteHub");
                });
            }
            else {
                app.UseSignalR(routes => {
                    routes.MapHub<VoteHub>("/voteHub");
                });
            }
        }
    }
}
