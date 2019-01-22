using System;
using AdventureJar.Web.Api.Middleware;
using AdventureJar.Web.DynamoService.Contracts;
using AdventureJar.Web.DynamoService.Tables;
using Amazon.DynamoDBv2;
using Amazon.S3;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Remotion.Linq.Clauses.ResultOperators;
using Swashbuckle.AspNetCore.Swagger;

namespace AdventureJar.Web.Api
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
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "AdventureJar.Web.Api",
                    Description = ".NET Core Web API integrating with Amazon DynamoDB",
                    Contact = new Contact
                    {
                        Name = "Heinrich Venter",
                        Email = "Heini141@outlook.com",
                        Url = "https://github.com/Heinrichv"
                    }
                });
            });
            services.AddResponseCompression();
            services.AddCors();
            services.AddMemoryCache();
            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            services.AddAWSService<IAmazonS3>();
            services.AddAWSService<IAmazonDynamoDB>();

            services.AddScoped<IActivity, Activity>();
            services.AddScoped<IAccount, Account>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseResponseCompression();
            app.UseCors(o =>
            {
                o.AllowAnyHeader();
                o.AllowAnyMethod();
                o.AllowAnyOrigin();
            });

            app.UseMiddleware<GlobalExceptionMiddleware>();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AdventureJar.Web.Api");
                c.RoutePrefix = string.Empty;
            });

            app.UseMvcWithDefaultRoute();

            ILoggerFactory loggerFactory = new LoggerFactory();
            loggerFactory.AddAWSProvider(this.Configuration.GetAWSLoggingConfigSection());
            ILogger<Startup> logger = loggerFactory.CreateLogger<Startup>();

            logger.LogCritical("Application Started");
        }
    }
}
