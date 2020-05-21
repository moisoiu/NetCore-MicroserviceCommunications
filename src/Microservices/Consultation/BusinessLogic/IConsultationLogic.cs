using DTO.Consultation;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Consultation.BusinessLogic
{
    public interface IConsultationLogic
    {
        Task<Guid> SaveConsultation(CreateConsultationCommand command);
        Task<List<GetConsultationResponse>> GetConsultations(ConsultationDto request);
        Task<T> GetConsultation<T>(Expression<Func<Entities.Consultation, bool>> predicate) where T : class;
        Task<bool> UpdateConsultation(Guid consultationId, Guid userId, JsonPatchDocument jsonPatchDocument);
    }
}
