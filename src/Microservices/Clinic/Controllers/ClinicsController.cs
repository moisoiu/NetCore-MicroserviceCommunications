using Clinic.BusinessLayer;
using DTO.Clinic;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClinicsController : ControllerBase
    {
        private readonly IClinicLogic clinicLogic;

        public ClinicsController(IClinicLogic clinicLogic)
        {
            this.clinicLogic = clinicLogic;
        }

        [HttpGet]
        public async Task<IActionResult> GetClinics([FromQuery]ClinicDto request)
        {
            var clinics = await clinicLogic.GetClinics(request);
            if (!clinics.Any())
            {
                return NotFound();
            }

            return Ok(clinics);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClinic([FromRoute]Guid id)
        {
            var clinic = await clinicLogic.GetClinic<GetClinicResponse>(x => x.Id == id);
            if (clinic == null)
            {
                return NotFound();
            }

            return Ok(clinic);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromHeader]Guid userId, [FromBody]CreateClinicCommand command)
        {
            var clinicId = await clinicLogic.SaveClinic(command);

            if (clinicId == Guid.Empty)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetClinic), new { id = clinicId }, clinicId);

        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch([FromRoute]Guid id, [FromHeader]Guid userId, [FromBody]IEnumerable<Operation> patchOperations)
        {
            var jsonPatchDocument = new JsonPatchDocument();
            jsonPatchDocument.Operations.AddRange(patchOperations);

            var isUpdated = await clinicLogic.UpdateClinic(id, userId, jsonPatchDocument);

            if (!isUpdated)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}
