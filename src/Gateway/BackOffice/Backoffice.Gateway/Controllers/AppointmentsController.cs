using AutoMapper;
using Backoffice.Gateway.Communications.Refit;
using Backoffice.Gateway.Models.Appointment.Requests;
using DTO.Appointment;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backoffice.Gateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentApi appointmentApi;
        private readonly IConsultationApi consultationApi;
        private readonly IClinicApi clinicApi;
        private readonly IPatientApi patientApi;
        private readonly IMapper mapper;

        public AppointmentsController(
            IAppointmentApi appointmentApi,
            IConsultationApi consultationApi,
            IClinicApi clinicApi,
            IPatientApi patientApi,
            IMapper mapper
            )
        {
            this.appointmentApi = appointmentApi;
            this.consultationApi = consultationApi;
            this.clinicApi = clinicApi;
            this.patientApi = patientApi;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAppointments([FromQuery]GetAppointmentRequest request)
        {
            var clinicsRequest = await appointmentApi.GetAppointments(request);

            if (!clinicsRequest.IsSuccessStatusCode)
            {
                return await clinicsRequest.GetActionResult();
            }

            var clinics = await clinicsRequest.Content.DeserializeStringContent<IEnumerable<GetAppointmentResponse>>();

            return Ok(clinics);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAppointmentById([FromRoute]Guid id)
        {
            var clinicRequest = await appointmentApi.GetAppointment(id);

            if (!clinicRequest.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var clinic = await clinicRequest.Content.DeserializeStringContent<GetAppointmentResponse>();

            return Ok(clinic);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Post([FromBody]CreateAppointmentRequest request)
        {
            var appointmentCommand = mapper.Map<CreateAppointmentCommand>(request);
            appointmentCommand.CreatedBy = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "id").Value);

            var getPatientTask = patientApi.GetPatient(request.PatientId);
            var getFromClinicTask = clinicApi.GetClinic(request.FromClinicId);
            var getToClinicTask = clinicApi.GetClinic(request.ToClinicId);

            await Task.WhenAll(getPatientTask, getFromClinicTask, getToClinicTask);

            var getPatientResultTask = getPatientTask.Result.Content.DeserializeStringContent<DTO.Patient.GetPatientResponse>();
            var getFromClinicResultTask = getFromClinicTask.Result.Content.DeserializeStringContent<DTO.Clinic.GetClinicResponse>();
            var getToClinicResultTask = getToClinicTask.Result.Content.DeserializeStringContent<DTO.Clinic.GetClinicResponse>();

            await Task.WhenAll(getPatientResultTask, getFromClinicResultTask, getToClinicResultTask);

            appointmentCommand.FromClinicName = getFromClinicResultTask.Result.Name;
            appointmentCommand.ToClinicName = getToClinicResultTask.Result.Name;

            appointmentCommand.PatientName = getPatientResultTask.Result.FirstName + " " + getPatientResultTask.Result.LastName;


            var consultationCreateResponse = await appointmentApi.SaveAppointment(appointmentCommand);

            var consultationCreateContent = await consultationCreateResponse.Content.DeserializeStringContent<string>();
            if (!consultationCreateResponse.IsSuccessStatusCode)
            {
                return BadRequest(consultationCreateContent);
            }

            return CreatedAtAction(nameof(GetAppointmentById), new { id = consultationCreateContent }, consultationCreateContent);
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Patch([FromRoute]Guid id, [FromBody]UpdateAppointmentRequest patchAppointment)
        {
            var patch = JsonPatchDocumentExtensions.CreatePatch(patchAppointment.Original, patchAppointment.Changed);

            var guidUserId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "id").Value);

            var patchResponse = await appointmentApi.PatchAppointment(id, guidUserId, patch.Operations);

            if (!patchResponse.IsSuccessStatusCode)
            {
                return await patchResponse.UpdateActionResult();
            }

            return NoContent(); // Because theoretically the user on frontend already has the data changed, no point in returning a new data 
        }
    }
}
