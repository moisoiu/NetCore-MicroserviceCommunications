using System;
using System.Collections.Generic;

namespace Appointment.Entities
{
    public partial class Appointment
    {
        public Guid Id { get; set; }
        public Guid ToClinicId { get; set; }
        public string ToClinicName { get; set; }
        public Guid FromClinicId { get; set; }
        public string FromClinicName { get; set; }
        public Guid PatientId { get; set; }
        public string PatientName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime Created { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime Updated { get; set; }
        public Guid UpdatedBy { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
