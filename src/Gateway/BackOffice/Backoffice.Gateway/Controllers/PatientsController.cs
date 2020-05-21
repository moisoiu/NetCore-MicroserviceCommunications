using AutoMapper;
using Backoffice.Gateway.Communications.Refit;
using Backoffice.Gateway.Models.Patient;
using DTO.Patient;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Backoffice.Gateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientApi patientApi;
        private readonly IMapper mapper;
        public PatientsController(
            IPatientApi patientApi,
            IMapper mapper)
        {
            this.patientApi = patientApi;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPatients([FromQuery]GetPatientsRequest request)
        {
            var patientsRequest = await patientApi.GetPatients(request);

            if (!patientsRequest.IsSuccessStatusCode)
            {
                return await patientsRequest.GetActionResult();
            }

            var patients = await patientsRequest.Content.DeserializeStringContent<IEnumerable<GetPatientResponse>>();


            return Ok(patients);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPatientById([FromRoute]Guid id)
        {
            var patientRequest = await patientApi.GetPatient(id);

            if (!patientRequest.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var patient = await patientRequest.Content.DeserializeStringContent<GetPatientResponse>();

            return Ok(patient);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Post([FromBody]CreatePatientRequest request)
        {
            var patientCommand = mapper.Map<CreatePatientCommand>(request);
            patientCommand.CreatedBy = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "id").Value);

            var patientCreateResponse = await patientApi.SavePatient(patientCommand);

            var patientCreateContent = await patientCreateResponse.Content.DeserializeStringContent<string>();

            if (!patientCreateResponse.IsSuccessStatusCode)
            {
                return BadRequest(patientCreateContent);
            }

            return CreatedAtAction(nameof(GetPatientById), new { id = patientCreateContent }, patientCreateContent);
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Patch([FromRoute]Guid id, [FromBody]UpdatePatientRequest patchUser)
        {
            var patch = JsonPatchDocumentExtensions.CreatePatch(patchUser.Original, patchUser.Changed);

            var guidUserId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "id").Value);

            var patchResponse = await patientApi.PatchPatient(id, guidUserId, patch.Operations);

            if (!patchResponse.IsSuccessStatusCode)
            {
                return await patchResponse.UpdateActionResult();
            }

            return NoContent(); // Because theoretically the user on frontend already has the data changed, no point in returning a new data 
        }
    }
}
