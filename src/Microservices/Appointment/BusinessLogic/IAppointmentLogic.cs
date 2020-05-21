using DTO.Appointment;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Appointment.BusinessLogic
{
    public interface IAppointmentLogic
    {
        Task<Guid> SaveAppointment(CreateAppointmentCommand command);
        Task<List<GetAppointmentResponse>> GetAppointments(AppointmentDto request);
        Task<T> GetAppointment<T>(Expression<Func<Entities.Appointment, bool>> predicate) where T : class;
        Task<bool> UpdateAppointment(Guid AppointmentId, Guid userId, JsonPatchDocument jsonPatchDocument);
    }
}
