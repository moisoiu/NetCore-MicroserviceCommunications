using System;
using System.Collections.Generic;

namespace Consultation.Entities
{
    public partial class Consultation
    {
        public Guid Id { get; set; }
        public Guid ClinicId { get; set; }
        public string ClinicName { get; set; }
        public Guid PatientId { get; set; }
        public string PatientName { get; set; }
        public bool IsActive { get; set; }
        public DateTime Created { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime Updated { get; set; }
        public Guid UpdatedBy { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
