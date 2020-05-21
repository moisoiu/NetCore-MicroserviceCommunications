
using AutoMapper;
using Backoffice.Gateway.Communications.Refit;
using Backoffice.Gateway.Models.Clinic;
using DTO.Clinic;
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
    public class ClinicsController : ControllerBase
    {
        private readonly IClinicApi clinicApi;
        private readonly IMapper mapper;
        public ClinicsController(
            IClinicApi clinicApi,
            IMapper mapper)
        {
            this.clinicApi = clinicApi;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetClinics([FromQuery]GetClinicsRequest request)
        {
            var clinicsRequest = await clinicApi.GetClinics(request);

            if (!clinicsRequest.IsSuccessStatusCode)
            {
                return await clinicsRequest.GetActionResult();
            }

            var clinics = await clinicsRequest.Content.DeserializeStringContent<IEnumerable<GetClinicResponse>>();

            return Ok(clinics);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetClinicById([FromRoute]Guid id)
        {
            var clinicRequest = await clinicApi.GetClinic(id);

            if (!clinicRequest.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var clinic = await clinicRequest.Content.DeserializeStringContent<GetClinicResponse>();

            return Ok(clinic);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Post([FromBody]CreateClinicRequest request)
        {
            var clinicCommand = mapper.Map<CreateClinicCommand>(request);
            clinicCommand.CreatedBy = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "id").Value);

            var clinicCreateResponse = await clinicApi.SaveClinic(clinicCommand);

            var clinicCreateContent = await clinicCreateResponse.Content.DeserializeStringContent<string>();

            if (!clinicCreateResponse.IsSuccessStatusCode)
            {
                return BadRequest(clinicCreateContent);
            }

            return CreatedAtAction(nameof(GetClinicById), new { id = clinicCreateContent }, clinicCreateContent);
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Patch([FromRoute]Guid id, [FromBody]UpdateClinicRequest patchClinic)
        {
            var patch = JsonPatchDocumentExtensions.CreatePatch(patchClinic.Original, patchClinic.Changed);

            var guidUserId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "id").Value);

            var patchResponse = await clinicApi.PatchClinics(id, guidUserId, patch.Operations);

            if (!patchResponse.IsSuccessStatusCode)
            {
                return await patchResponse.UpdateActionResult();
            }

            return NoContent(); // Because theoretically the user on frontend already has the data changed, no point in returning a new data 
        }
    }
}
