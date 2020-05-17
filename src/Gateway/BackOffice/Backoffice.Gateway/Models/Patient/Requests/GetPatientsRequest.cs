using DTO.Patient;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backoffice.Gateway.Models.Patient
{
    public class GetPatientsRequest : PatientDto
    {
    }

    public class GetPatientsRequestValidator : AbstractValidator<GetPatientsRequest>
    {
        public GetPatientsRequestValidator()
        {
            RuleFor(x => x)
                .Must(x =>
                    x.DateOfBirth != default ||                    
                    !string.IsNullOrEmpty(x.FirstName) ||
                    !string.IsNullOrEmpty(x.LastName)
                )
                .WithMessage("Must complete at least one field");
        }
    }
}
