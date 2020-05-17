using AutoMapper;
using AutoMapper.QueryableExtensions;
using Clinic.Entities;
using DTO.Clinic;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Clinic.BusinessLayer
{
    public class ClinicLogic : IClinicLogic
    {
        private readonly IMapper mapper;
        private readonly ClinicContext context;

        public ClinicLogic(IMapper mapper, ClinicContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public Task<T> GetClinic<T>(Expression<Func<Entities.Clinic, bool>> predicate) where T : class
        {
            return context
              .Clinic
              .Where(predicate)
              .ProjectTo<T>(mapper.ConfigurationProvider)
              .FirstOrDefaultAsync();
        }

        public Task<List<GetClinicResponse>> GetClinics(ClinicDto request)
        {
            var name = request.Name;
            var isActive = request.IsActive;

            return context
                .Clinic
                .AsNoTracking()
                .ConditionalWhere(!string.IsNullOrEmpty(name), x => x.Name == name)
                .ConditionalWhere(isActive != null, x => x.IsActive == isActive.Value)
                .ProjectTo<GetClinicResponse>(mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<Guid> SaveClinic(CreateClinicCommand command)
        {
            var patient = mapper.Map<Entities.Clinic>(command);
            patient.Id = Guid.NewGuid();
            patient.IsActive = true;
            patient.Created = DateTime.UtcNow;
            patient.CreatedBy = command.CreatedBy;
            patient.Updated = DateTime.UtcNow;
            patient.UpdateBy = command.CreatedBy;

            await context.Clinic.AddAsync(patient);
            var rowChanged = await context.SaveChangesAsync();

            if (rowChanged == 0)
                return Guid.Empty;

            return patient.Id;
        }

        public async Task<bool> UpdateClinic(Guid clinicId, Guid userId, JsonPatchDocument jsonPatchDocument)
        {
            var clinic = await context
                .Clinic
                .FirstOrDefaultAsync(x => x.Id == clinicId);

            jsonPatchDocument.ApplyTo(clinic);

            clinic.UpdateBy = userId;
            clinic.Updated = DateTime.UtcNow;

            context.Clinic.Update(clinic);
            var rowChanged = await context.SaveChangesAsync();
            if (rowChanged == 0)
                return false;

            return true;
        }
    }
}
