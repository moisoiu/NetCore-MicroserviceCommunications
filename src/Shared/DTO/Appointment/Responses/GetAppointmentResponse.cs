using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Appointment
{
    public class GetAppointmentResponse : AppointmentDto
    {
        public Guid Id { get; set; }
    }
}
