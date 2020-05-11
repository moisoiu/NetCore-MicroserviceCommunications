using Backoffice.Gateway.Models.Patient;
using DTO.Patient;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Backoffice.Gateway.Communications.Refit
{
    public interface IPatientApi
    {
        [Get("/api/patients/{id}")]
        Task<HttpResponseMessage> GetPatient(Guid id);

        [Get("/api/patients")]
        Task<HttpResponseMessage> GetPatients([Query]GetPatientsRequest request);

        [Post("/api/patients")]
        Task<HttpResponseMessage> SavePatient([Body]CreatePatientCommand command);

        [Patch("/api/patients/{id}")]
        Task<HttpResponseMessage> PatchPatient(Guid id,[Header("userId")]Guid userId, [Body]IEnumerable<Operation> patchUser);
    }
}
