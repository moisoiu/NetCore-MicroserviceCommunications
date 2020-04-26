using System;
using System.Collections.Generic;

namespace CommunicationConfig.Entities
{
    internal partial class Communication
    {
        public int Id { get; set; }
        public string CommunicationModeName { get; set; }
        public bool IsActive { get; set; }
    }
}
