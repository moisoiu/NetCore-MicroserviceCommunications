using AutoMapper;
using AutoMapper.QueryableExtensions;
using DTO.Patient;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Patient.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Patient.BusinessLayer
{
    public class PatientLogic : IPatientLogic
    {
        private readonly IMapper mapper;
        private readonly PatientContext context;

        public PatientLogic(IMapper mapper, PatientContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public Task<T> GetPatient<T>(Expression<Func<Entities.Patient, bool>> predicate) where T : class
        {
            return context
              .Patient
              .Where(predicate)
              .ProjectTo<T>(mapper.ConfigurationProvider)
              .FirstOrDefaultAsync();
        }

        public Task<List<GetPatientResponse>> GetPatients(PatientDto request)
        {
            var firstName = request.FirstName;
            var lastName = request.LastName;            

            return context
                .Patient   
                .AsNoTracking()
                .ConditionalWhere(!string.IsNullOrEmpty(firstName), x => x.FirstName == firstName)
                .ConditionalWhere(!string.IsNullOrEmpty(lastName), x => x.LastName == lastName)
                .ConditionalWhere(request.DateOfBirth != default(DateTime), x => x.DateOfBirth == request.DateOfBirth)
                .ProjectTo<GetPatientResponse>(mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<Guid> SavePatient(CreatePatientCommand command)
        {
            var patient = mapper.Map<Entities.Patient>(command);
            patient.Id = Guid.NewGuid();
            patient.IsActive = true;
            patient.Created = DateTime.UtcNow;
            patient.CreatedBy = command.CreatedBy;
            patient.Updated = DateTime.UtcNow;
            patient.UpdatedBy = command.CreatedBy;

            await context.Patient.AddAsync(patient);
            var rowChanged = await context.SaveChangesAsync();

            if (rowChanged == 0)
                return Guid.Empty;

            return patient.Id;
        }

        public async Task<bool> UpdatePatient(Guid patientId, Guid userId, JsonPatchDocument jsonPatchDocument)
        {
            var patient = await context
                .Patient
                .FirstOrDefaultAsync(x => x.Id == patientId);

            jsonPatchDocument.ApplyTo(patient);

            patient.UpdatedBy = userId;
            patient.Updated = DateTime.UtcNow;

            context.Patient.Update(patient);
            var rowChanged = await context.SaveChangesAsync();
            if (rowChanged == 0)
                return false;

            return true;
        }
    }
}
