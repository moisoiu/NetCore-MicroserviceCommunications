using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using CommunicationConfig.Enums;
using CommunicationConfig;
using Backoffice.Gateway.Communications.Refit;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using Infrastructure.Extensions;
using IdentityServer4.AccessTokenValidation;
using DTO.Configuration;
using FluentValidation.AspNetCore;
using DTO.User;
using AutoMapper;

namespace Backoffice.Gateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Settings = configuration.Get<Settings>();
        }

        public IConfiguration Configuration { get; }
        public Settings Settings { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Settings);

            SetupIdentityServerAuthentication(services);

            SetupSwagger(services);

            services.AddControllers()
                .AddNewtonsoftJson()
                .AddFluentValidation(config => 
                {
                    config.RegisterValidatorsFromAssemblyContaining<CreateUserRequest>();
                    config.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                });


            SetupCommunicationMode(services);

            services.AddAutoMapper(typeof(Startup));


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseErrorHandling();

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.DisplayRequestDuration();

                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// Configure in what way Gateway will communicate with the other services / microservices
        /// </summary>
        /// <param name="services"></param>
        private void SetupCommunicationMode(IServiceCollection services)
        {
            var communicationMode = Communication.GetCommunicationMode(services, Configuration.GetConnectionString("Configuration"));

            switch (communicationMode)
            {
                case CommunicationMode.AkkaNet:
                    break;

                case CommunicationMode.AzureEventBus:
                    break;

                case CommunicationMode.AzureServiceBus:
                    break;

                case CommunicationMode.Dapr:
                    break;

                case CommunicationMode.gRPC:
                    break;

                case CommunicationMode.Kafka:
                    break;

                case CommunicationMode.NServiceBus:
                    break;

                case CommunicationMode.Refit:
                    services.InitiateRefitServices(Configuration);
                    break;

                case CommunicationMode.SagaWorkFlow:
                    break;

                case CommunicationMode.OData:
                    break;

                default:
                    throw new NotSupportedException("No communication mode supported");
            }
        }

        private void SetupIdentityServerAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
               .AddIdentityServerAuthentication(options =>
               {
                   options.Authority = Settings.AuthenticationServer.Authority;
                   options.ApiName = Settings.AuthenticationServer.ApiName;
                   options.RequireHttpsMetadata = Settings.AuthenticationServer.RequiresHttps;
                   options.JwtBearerEvents = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents()
                   {
                       // Apply different validation if you want to create a blacklist for JWT or can be done via Middleware 
                       OnTokenValidated = (context) =>
                       {
                           return Task.CompletedTask;
                       }
                   };
               });
        }

        private void SetupSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Backoffice.Gateway", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme.",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });
            });
        }
    }
}
