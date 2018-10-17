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
using Pollr.Api;
using Pollr.Api.Dal;
using Pollr.Api.Hubs;

namespace pollr.api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
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

            services.Configure<Settings>(options =>
            {
                options.ConnectionString = Configuration.GetSection("MongoConnection:ConnectionString").Value;
                options.Database = Configuration.GetSection("MongoConnection:Database").Value;
            });

            // Add a SignalR hub
            var signalrKey = Configuration.GetSection("Azure:SignalR:ConnectionString").Value;
            services.AddSignalR()
                    .AddAzureSignalR("Endpoint=https://jrd-pollr-hub.service.signalr.net;AccessKey=MgdlZniHHD/TGroykFmanT/Dy/MwPQRslt3COZTwCrs=;Version=1.0;");

            services.AddTransient<IPollDefinitionRepository, PollDefinitionRepository>();
            services.AddTransient<IPollRepository, PollRepository>();

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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
            //app.UseSignalR(routes => {
            //    routes.MapHub<VoteHub>("/voteHub");
            //});

            app.UseAzureSignalR(routes =>
            {
                routes.MapHub<VoteHub>("/voteHub");
            });
        }
    }
}
