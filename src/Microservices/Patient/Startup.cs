using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CommunicationConfig;
using CommunicationConfig.Enums;
using DTO.Patient;
using FluentValidation.AspNetCore;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Patient.BusinessLayer;
using Patient.Entities;

namespace Patient
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
            var connectionString = Configuration.GetConnectionString("Patient");

            services.AddDbContext<PatientContext>(options =>
            {
                options.UseSqlServer(connectionString);
            },
            ServiceLifetime.Transient);

            services.AddControllers()
                 .AddNewtonsoftJson()
                 .AddFluentValidation(config =>
                 {
                     config.RegisterValidatorsFromAssemblyContaining<CreatePatientCommand>();
                     config.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                     config.ImplicitlyValidateChildProperties = true;
                 });

            services.AddAutoMapper(typeof(Startup));

            services.AddTransient<IPatientLogic, PatientLogic>();

            services.SetupDomainDatabase<PatientContext>();

            SetupCommunicationMode(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseErrorHandling();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Patient Domain - Up & Running");
                });
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
                    break;

                case CommunicationMode.SagaWorkFlow:
                    break;

                default:
                    throw new NotSupportedException("No communication mode supported");
            }
        }
    }
}
