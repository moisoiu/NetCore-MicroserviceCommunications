using DTO.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;

namespace Backoffice.Gateway.Communications.Refit
{
    public static class Setup
    {
        public static void InitiateRefitServices(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettingsOption = configuration.Get<Settings>();

            var settings = new RefitSettings
            {
                
            };

            services.AddRefitClient<IUserApi>(settings)
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri(appSettingsOption.RefitUrls.UserApi);

                });

            services.AddRefitClient<IPatientApi>(settings)
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri(appSettingsOption.RefitUrls.PatientApi);

                });

            services.AddRefitClient<IClinicApi>(settings)
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri(appSettingsOption.RefitUrls.ClinicApi);

                });

            services.AddRefitClient<IConsultationApi>(settings)
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri(appSettingsOption.RefitUrls.ConsultationApi);

                });

            services.AddRefitClient<IAppointmentApi>(settings)
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri(appSettingsOption.RefitUrls.AppointmentApi);

                });
        }
    }
}
