using Backoffice.Gateway.Models.Clinic;
using DTO.Clinic;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Backoffice.Gateway.Communications.Refit
{
    public interface IClinicApi
    {
        [Get("/api/clinics/{id}")]
        Task<HttpResponseMessage> GetClinic(Guid id);

        [Get("/api/clinics")]
        Task<HttpResponseMessage> GetClinics([Query]GetClinicsRequest request);

        [Post("/api/clinics")]
        Task<HttpResponseMessage> SaveClinic([Body]CreateClinicCommand command);

        [Patch("/api/clinics/{id}")]
        Task<HttpResponseMessage> PatchClinics(Guid id, [Header("userId")]Guid userId, [Body]IEnumerable<Operation> patchUser);
    }
}
