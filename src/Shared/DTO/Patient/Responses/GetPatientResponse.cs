using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Patient
{
    public class GetPatientResponse : PatientDto
    {
        public Guid Id { get; set; }
    }
}
