using Backoffice.Gateway.Models.Consultation;
using DTO.Consultation;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Backoffice.Gateway.Communications.Refit
{
    public interface IConsultationApi
    {
        [Get("/api/consultations/{id}")]
        Task<HttpResponseMessage> GetConsultation(Guid id);

        [Get("/api/consultations")]
        Task<HttpResponseMessage> GetConsultations([Query]GetConsultationRequest request);

        [Post("/api/consultations")]
        Task<HttpResponseMessage> SaveConsultation([Body]CreateConsultationCommand command);

        [Patch("/api/consultations/{id}")]
        Task<HttpResponseMessage> PatchConsultations(Guid id, [Header("userId")]Guid userId, [Body]IEnumerable<Operation> patchUser);
    }
}
