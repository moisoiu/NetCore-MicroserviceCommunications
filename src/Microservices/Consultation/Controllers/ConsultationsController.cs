using Consultation.BusinessLogic;
using DTO.Consultation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consultation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultationsController : ControllerBase
    {
        private readonly IConsultationLogic consultationLogic;

        public ConsultationsController(IConsultationLogic consultationLogic)
        {
            this.consultationLogic = consultationLogic;
        }

        [HttpGet]
        public async Task<IActionResult> GetConsultations([FromQuery]ConsultationDto request)
        {
            var consultations = await consultationLogic.GetConsultations(request);
            if (!consultations.Any())
            {
                return NotFound();
            }

            return Ok(consultations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetConsultation([FromRoute]Guid id)
        {
            var consultation = await consultationLogic.GetConsultation<GetConsultationResponse>(x => x.Id == id);
            if (consultation == null)
            {
                return NotFound();
            }

            return Ok(consultation);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromHeader]Guid userId, [FromBody]CreateConsultationCommand command)
        {
            var consultationId = await consultationLogic.SaveConsultation(command);

            if (consultationId == Guid.Empty)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetConsultation), new { id = consultationId }, consultationId);

        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch([FromRoute]Guid id, [FromHeader]Guid userId, [FromBody]IEnumerable<Operation> patchOperations)
        {
            var jsonPatchDocument = new JsonPatchDocument();
            jsonPatchDocument.Operations.AddRange(patchOperations);

            var isUpdated = await consultationLogic.UpdateConsultation(id, userId, jsonPatchDocument);

            if (!isUpdated)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}

