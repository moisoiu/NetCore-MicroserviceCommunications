using System;
using System.Collections.Generic;

namespace Clinic.Entities
{
    public partial class Clinic
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime Created { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime Updated { get; set; }
        public Guid UpdateBy { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
