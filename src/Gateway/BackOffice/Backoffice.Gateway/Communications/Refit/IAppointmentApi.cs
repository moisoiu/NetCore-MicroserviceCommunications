using Backoffice.Gateway.Models.Appointment.Requests;
using DTO.Appointment;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Backoffice.Gateway.Communications.Refit
{
    public interface IAppointmentApi
    {
        [Get("/api/appointments/{id}")]
        Task<HttpResponseMessage> GetAppointment(Guid id);

        [Get("/api/appointments")]
        Task<HttpResponseMessage> GetAppointments([Query]GetAppointmentRequest request);

        [Post("/api/appointments")]
        Task<HttpResponseMessage> SaveAppointment([Body]CreateAppointmentCommand command);

        [Patch("/api/appointments/{id}")]
        Task<HttpResponseMessage> PatchAppointment(Guid id, [Header("userId")]Guid userId, [Body]IEnumerable<Operation> patchUser);
    }
}
