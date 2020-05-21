using Appointment.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DTO.Appointment;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Appointment.BusinessLogic
{
    public class AppointmentLogic : IAppointmentLogic
    {
        private readonly IMapper mapper;
        private readonly AppointmentContext context;

        public AppointmentLogic(IMapper mapper, AppointmentContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public Task<T> GetAppointment<T>(Expression<Func<Entities.Appointment, bool>> predicate) where T : class
        {
            return context
              .Appointment
              .Where(predicate)
              .ProjectTo<T>(mapper.ConfigurationProvider)
              .FirstOrDefaultAsync();
        }

        public Task<List<GetAppointmentResponse>> GetAppointments(AppointmentDto request)
        {
            var endDate = request.EndDate;
            var fromClinicId = request.FromClinicId;
            var fromClinicName = request.FromClinicName;
            var isActive = request.IsActive;
            var patientId = request.PatientId;
            var patientName = request.PatientName;
            var startDate = request.StartDate;
            var toClinicId = request.ToClinicId;
            var toClinicName = request.ToClinicName;            

            return context
                .Appointment
                .AsNoTracking()
                .ConditionalWhere(endDate != default && startDate == default, x => x.EndDate == endDate)
                .ConditionalWhere(startDate != default && endDate == default, x => x.StartDate == startDate)
                .ConditionalWhere(startDate != default && endDate != default, x => x.StartDate >= startDate && x.EndDate <= x.EndDate)
                .ConditionalWhere(fromClinicId != Guid.Empty, x => x.FromClinicId == fromClinicId)
                .ConditionalWhere(!string.IsNullOrEmpty(fromClinicName), x => x.FromClinicName == fromClinicName)
                .ConditionalWhere(isActive.HasValue, x => x.IsActive == isActive.Value)
                .ConditionalWhere(patientId != Guid.Empty, x => x.IsActive == isActive.Value)
                .ConditionalWhere(!string.IsNullOrEmpty(patientName), x => x.PatientName == patientName)
                .ConditionalWhere(toClinicId != Guid.Empty, x => x.ToClinicId == toClinicId)
                .ConditionalWhere(!string.IsNullOrEmpty(toClinicName), x => x.ToClinicName == toClinicName)
                .ProjectTo<GetAppointmentResponse>(mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<Guid> SaveAppointment(CreateAppointmentCommand command)
        {
            var appointment = mapper.Map<Entities.Appointment>(command);
            appointment.Id = Guid.NewGuid();
            appointment.IsActive = true;
            appointment.Created = DateTime.UtcNow;
            appointment.CreatedBy = command.CreatedBy;
            appointment.Updated = DateTime.UtcNow;
            appointment.UpdatedBy = command.CreatedBy;

            await context.Appointment.AddAsync(appointment);
            var rowChanged = await context.SaveChangesAsync();

            if (rowChanged == 0)
                return Guid.Empty;

            return appointment.Id;
        }

        public async Task<bool> UpdateAppointment(Guid appointmentId, Guid userId, JsonPatchDocument jsonPatchDocument)
        {
            var appointment = await context
               .Appointment
               .FirstOrDefaultAsync(x => x.Id == appointmentId);

            jsonPatchDocument.ApplyTo(appointment);

            appointment.UpdatedBy = userId;
            appointment.Updated = DateTime.UtcNow;

            context.Appointment.Update(appointment);
            var rowChanged = await context.SaveChangesAsync();
            if (rowChanged == 0)
                return false;

            return true;
        }
    }
}
