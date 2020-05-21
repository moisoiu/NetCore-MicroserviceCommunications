using AutoMapper;
using AutoMapper.QueryableExtensions;
using Consultation.Entities;
using DTO.Consultation;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Consultation.BusinessLogic
{
    public class ConsultationLogic : IConsultationLogic
    {
        private readonly IMapper mapper;
        private readonly ConsultationContext context;
        public ConsultationLogic(IMapper mapper, ConsultationContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public Task<List<GetConsultationResponse>> GetConsultations(ConsultationDto request)
        {
            var clinicName = request.ClinicName;
            var patientName = request.PatientName;

            return context
                .Consultation
                .AsNoTracking()
                .ConditionalWhere(!string.IsNullOrEmpty(clinicName), x => x.ClinicName == clinicName)
                .ConditionalWhere(!string.IsNullOrEmpty(patientName), x => x.PatientName == patientName)
                .ProjectTo<GetConsultationResponse>(mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public Task<T> GetConsultation<T>(Expression<Func<Entities.Consultation, bool>> predicate) where T : class
        {
            return context
              .Consultation
              .Where(predicate)
              .ProjectTo<T>(mapper.ConfigurationProvider)
              .FirstOrDefaultAsync();
        }

        public async Task<Guid> SaveConsultation(CreateConsultationCommand command)
        {
            var consultation = mapper.Map<Entities.Consultation>(command);
            consultation.Id = Guid.NewGuid();
            consultation.IsActive = true;
            consultation.Created = DateTime.UtcNow;
            consultation.CreatedBy = command.CreatedBy;
            consultation.Updated = DateTime.UtcNow;
            consultation.UpdatedBy = command.CreatedBy;

            await context.Consultation.AddAsync(consultation);
            var rowChanged = await context.SaveChangesAsync();

            if (rowChanged == 0)
                return Guid.Empty;

            return consultation.Id;
        }

        public async Task<bool> UpdateConsultation(Guid consultationId, Guid userId, JsonPatchDocument jsonPatchDocument)
        {
            var consultation = await context
               .Consultation
               .FirstOrDefaultAsync(x => x.Id == consultationId);

            jsonPatchDocument.ApplyTo(consultation);

            consultation.UpdatedBy = userId;
            consultation.Updated = DateTime.UtcNow;

            context.Consultation.Update(consultation);
            var rowChanged = await context.SaveChangesAsync();
            if (rowChanged == 0)
                return false;

            return true;
        }
    }
}
