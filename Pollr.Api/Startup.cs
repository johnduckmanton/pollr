/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Pollr.Api.Data;
using Pollr.Api.Exceptions;
using Pollr.Api.Hubs;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Reflection;

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
			// Enable IOptions in the DI container
			services.AddOptions();

			// SQL Server Database
			// string connectionString = Configuration.GetConnectionString("PollrDatabase");
			string connectionString = Configuration["PollrDatabase"];
            _logger.LogInformation($"### Connection String={connectionString}");

            if (string.IsNullOrEmpty(connectionString))
			{
				_logger.LogError("### Connection String PollrDatabase not configured.");

				throw new AppConfigErrorException("Database connection string is not configured");
			}

			services.AddDbContext<PollrContext>
					(options => options
					.UseSqlServer(connectionString,
							sqlServerOptionsAction: sqlOptions =>
							{
								sqlOptions.EnableRetryOnFailure(
											maxRetryCount: 10,
											maxRetryDelay: TimeSpan.FromSeconds(30),
											errorNumbersToAdd: null);
							}));

			// Automatically perform database migration
			// *** You may not want to do this in a real production app ***
			services.BuildServiceProvider().GetService<PollrContext>().Database.Migrate();

			//services.AddAuthentication(AzureADDefaults.BearerAuthenticationScheme)
			//    .AddAzureADBearer(options => Configuration.Bind("AzureAd", options));

			services.AddCors(o => o.AddPolicy("AllowAny", builder =>
			{
				builder.AllowAnyOrigin()
									.AllowAnyOrigin()
									.AllowAnyMethod()
									.AllowAnyHeader()
									.AllowCredentials();
			}));

			// Add a SignalR hub:
			// In production we will typically use an Azure Managed hub, but in development
			// we''l just create a local hub
			if (!bool.TryParse(Configuration.GetSection("SignalR:UseAzureSignalRManagedHub").Value, out useAzureSignalRManagedHub))
			{
				useAzureSignalRManagedHub = false;
			}

			if (useAzureSignalRManagedHub)
			{
				_logger.LogInformation("### Using Azure Managed SignaR hub.");
				services.AddSignalR()
								.AddAzureSignalR(Configuration.GetSection("SignalR:Azure:SignalR:ConnectionString").Value);
			}
			else
			{
				_logger.LogInformation("### Using local SignaR hub.");
				services.AddSignalR();
			}

			services.AddMvc()
					.AddJsonOptions(options =>
					{
						options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
						options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
					})
					.SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			// Register the Swagger generator, defining 1 or more Swagger documents
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new Info
				{
					Version = "v1",
					Title = "Pollr API",
					Description = "ASP.NET Core Web API for the Pollr Demo Application",
					TermsOfService = "None",
					Contact = new Contact
					{
						Name = "John Duckmanton",
						Email = "john.duckmanton@microsoft.com",
						Url = "https://github.com/johnduckmanton/pollr"
					},
					License = new License
					{
						Name = "Licensed under the MIT License"
					}
				});

				// Set the comments path for the Swagger JSON and UI.
				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				c.IncludeXmlComments(xmlPath);

			});

			services.AddTransient<IPollDefinitionRepository, PollDefinitionRepository>();
			services.AddTransient<IPollRepository, PollRepository>();

		}


		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{

			loggerFactory
					.AddConsole()
					.AddDebug()
					.AddAzureWebAppDiagnostics();

            // Enable Global Cors: Don't do this is a real production app!
            app.UseCors("AllowAny");

            _logger.LogInformation($"### Environment: {env.EnvironmentName}");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // If we are running in a Docker container we can assume that the 
                // SSL termination will bew handled outside the container
                if (!env.IsEnvironment("Docker"))
                {
                    //app.UseExceptionHandler("/Error");
                    //app.UseHsts();
                    //app.UseHttpsRedirection();
                }
            }

			//app.UseAuthentication();

			// Enable middleware to serve generated Swagger as a JSON endpoint.
			app.UseSwagger();

			// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
			// specifying the Swagger JSON endpoint.
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pollr API V1");
			});

			app.UseMvc();

			// Configure Signalr
			if (useAzureSignalRManagedHub)
			{
				app.UseAzureSignalR(routes =>
				{
					routes.MapHub<VoteHub>("/votehub");
				});
			}
			else
			{
				app.UseSignalR(routes =>
				{
					routes.MapHub<VoteHub>("/votehub");
				});
			}
		}
	}
}
