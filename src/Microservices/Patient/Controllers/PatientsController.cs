using DTO.Patient;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Patient.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Patient.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientLogic patientLogic;

        public PatientsController(IPatientLogic patientLogic)
        {
            this.patientLogic = patientLogic;
        }

        [HttpGet]
        public async Task<IActionResult> GetPatients([FromQuery]PatientDto request)
        {
            var patients = await patientLogic.GetPatients(request);
            if(!patients.Any())
            {
                return NotFound();
            }

            return Ok(patients);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatient([FromRoute]Guid id)
        {
            var patient = await patientLogic.GetPatient<GetPatientResponse>(x => x.Id == id);
            if(patient == null)
            {
                return NotFound();
            }

            return Ok(patient);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromHeader]Guid userId, [FromBody]CreatePatientCommand command)
        {
            var patientId = await patientLogic.SavePatient(command);

            if (patientId == Guid.Empty)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetPatient), new { id = patientId }, patientId);

        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch([FromRoute]Guid id, [FromHeader]Guid userId, [FromBody]IEnumerable<Operation> patchOperations)
        {
            var jsonPatchDocument = new JsonPatchDocument();
            jsonPatchDocument.Operations.AddRange(patchOperations);

            var isUpdated = await patientLogic.UpdatePatient(id, userId, jsonPatchDocument);

            if (!isUpdated)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}
