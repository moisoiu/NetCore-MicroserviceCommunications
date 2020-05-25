using System;
using System.IO;
using AutoMapper;
using CommunicationConfig;
using CommunicationConfig.Enums;
using DTO.User;
using FluentValidation.AspNetCore;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using User.BusinessLayer;
using User.Entities;
using User.Identity;

namespace User
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
            var connectionString = Configuration.GetConnectionString("User");

            //https://vmsdurano.com/apiboilerplate-and-identityserver4-access-control-for-apis/
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryApiResources(ResourceManager.Apis())
                .AddInMemoryClients(ClientManager.Clients())
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                .AddProfileService<ProfileService>();

            services.AddDbContext<UserContext>(options =>
            {
                options.UseSqlServer(connectionString);
            },
            ServiceLifetime.Transient);

            services.AddControllers()
                 .AddNewtonsoftJson()
                 .AddFluentValidation(config =>
                 {
                     config.RegisterValidatorsFromAssemblyContaining<CreateUserCommand>();
                     config.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                     config.ImplicitlyValidateChildProperties = true;
                 });

            services.AddAutoMapper(typeof(Startup));

            services.AddTransient<IUserLogic, UserLogic>();

            SetupCommunicationMode(services);

            InitializeDatabase(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseErrorHandling();

            app.UseIdentityServer();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("User Domain - Up & Running");
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

        /// <summary>
        /// Ensure Database created and adds an User
        /// </summary>
        /// <param name="serviceCollection"></param>
        private void InitializeDatabase(IServiceCollection serviceCollection)
        {
            if (serviceCollection.SetupDomainDatabase<UserContext>())
            {
                var serviceProvider = serviceCollection.BuildServiceProvider();
                var userLogic = serviceProvider.GetRequiredService<IUserLogic>();
                var createUserCommand = new CreateUserCommand()
                {
                    Account = "mm00001",
                    Email = "random@yahoo.com",
                    FirstName = "Mircea",
                    LastName = "Moisoiu",
                    Password = "Password1!"
                };

                userLogic.SaveUser(createUserCommand);
            }
        }
    }
}
