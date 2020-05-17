using DTO.Clinic;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Clinic.BusinessLayer
{
    public interface IClinicLogic
    {
        Task<Guid> SaveClinic(CreateClinicCommand command);
        Task<List<GetClinicResponse>> GetClinics(ClinicDto request);
        Task<T> GetClinic<T>(Expression<Func<Entities.Clinic, bool>> predicate) where T : class;
        Task<bool> UpdateClinic(Guid clinicId, Guid userId, JsonPatchDocument jsonPatchDocument);
    }
}
