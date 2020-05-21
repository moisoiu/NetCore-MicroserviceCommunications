using Appointment.BusinessLogic;
using DTO.Appointment;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentLogic appointmentLogic;

        public AppointmentsController(IAppointmentLogic appointmentLogic)
        {
            this.appointmentLogic = appointmentLogic;
        }

        [HttpGet]
        public async Task<IActionResult> GetAppointments([FromQuery]AppointmentDto request)
        {
            var appointments = await appointmentLogic.GetAppointments(request);
            if (!appointments.Any())
            {
                return NotFound();
            }

            return Ok(appointments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointment([FromRoute]Guid id)
        {
            var appointment = await appointmentLogic.GetAppointment<GetAppointmentResponse>(x => x.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return Ok(appointment);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromHeader]Guid userId, [FromBody]CreateAppointmentCommand command)
        {
            var appointmentId = await appointmentLogic.SaveAppointment(command);

            if (appointmentId == Guid.Empty)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetAppointment), new { id = appointmentId }, appointmentId);

        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch([FromRoute]Guid id, [FromHeader]Guid userId, [FromBody]IEnumerable<Operation> patchOperations)
        {
            var jsonPatchDocument = new JsonPatchDocument();
            jsonPatchDocument.Operations.AddRange(patchOperations);

            var isUpdated = await appointmentLogic.UpdateAppointment(id, userId, jsonPatchDocument);

            if (!isUpdated)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}