using DTO.Patient;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Patient.BusinessLayer
{
    public interface IPatientLogic
    {
        Task<Guid> SavePatient(CreatePatientCommand command);
        Task<List<GetPatientResponse>> GetPatients(PatientDto request);
        Task<T> GetPatient<T>(Expression<Func<Entities.Patient, bool>> predicate) where T : class;
        Task<bool> UpdatePatient(Guid patientId, Guid userId, JsonPatchDocument jsonPatchDocument);
    }
}
