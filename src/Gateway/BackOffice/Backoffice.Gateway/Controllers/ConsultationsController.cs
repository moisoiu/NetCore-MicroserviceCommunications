using AutoMapper;
using Backoffice.Gateway.Communications.Refit;
using Backoffice.Gateway.Models.Consultation;
using DTO.Consultation;
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
    public class ConsultationsController : ControllerBase
    {
        private readonly IConsultationApi consultationApi;
        private readonly IClinicApi clinicApi;
        private readonly IPatientApi patientApi;
        private readonly IMapper mapper;
        public ConsultationsController(
            IConsultationApi consultationApi,
            IMapper mapper,
            IClinicApi clinicApi,
            IPatientApi patientApi)
        {
            this.consultationApi = consultationApi;
            this.mapper = mapper;
            this.clinicApi = clinicApi;
            this.patientApi = patientApi;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetConsultations([FromQuery]GetConsultationRequest request)
        {
            var clinicsRequest = await consultationApi.GetConsultations(request);

            if (!clinicsRequest.IsSuccessStatusCode)
            {
                return await clinicsRequest.GetActionResult();
            }

            var clinics = await clinicsRequest.Content.DeserializeStringContent<IEnumerable<GetConsultationResponse>>();

            return Ok(clinics);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetConsultationById([FromRoute]Guid id)
        {
            var clinicRequest = await consultationApi.GetConsultation(id);

            if (!clinicRequest.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var clinic = await clinicRequest.Content.DeserializeStringContent<GetConsultationResponse>();

            return Ok(clinic);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Post([FromBody]CreateConsultationRequest request)
        {
            var consultationCommand = mapper.Map<CreateConsultationCommand>(request);
            consultationCommand.CreatedBy = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "id").Value);

            var getPatientTask = patientApi.GetPatient(request.PatientId);
            var getClinicTask = clinicApi.GetClinic(request.ClinicId);

            await Task.WhenAll(getPatientTask, getClinicTask);

            var getPatientResultTask = getPatientTask.Result.Content.DeserializeStringContent<DTO.Patient.GetPatientResponse>();
            var getClinicResultTask = getClinicTask.Result.Content.DeserializeStringContent<DTO.Clinic.GetClinicResponse>();

            await Task.WhenAll(getPatientResultTask, getClinicResultTask);

            consultationCommand.ClinicName = getClinicResultTask.Result.Name;
            consultationCommand.PatientName = getPatientResultTask.Result.FirstName + " " + getPatientResultTask.Result.LastName;


            var consultationCreateResponse = await consultationApi.SaveConsultation(consultationCommand);

            var consultationCreateContent = await consultationCreateResponse.Content.DeserializeStringContent<string>();
            if (!consultationCreateResponse.IsSuccessStatusCode)
            {
                return BadRequest(consultationCreateContent);
            }

            return CreatedAtAction(nameof(GetConsultationById), new { id = consultationCreateContent }, consultationCreateContent);
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Patch([FromRoute]Guid id, [FromBody]UpdateConsultationRequest patchConsultation)
        {
            var patch = JsonPatchDocumentExtensions.CreatePatch(patchConsultation.Original, patchConsultation.Changed);

            var guidUserId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "id").Value);

            var patchResponse = await consultationApi.PatchConsultations(id, guidUserId, patch.Operations);

            if (!patchResponse.IsSuccessStatusCode)
            {
                return await patchResponse.UpdateActionResult();
            }

            return NoContent(); // Because theoretically the user on frontend already has the data changed, no point in returning a new data 
        }
    }
}
