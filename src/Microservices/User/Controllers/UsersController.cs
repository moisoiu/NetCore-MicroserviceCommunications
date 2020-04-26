using DTO.User;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.BusinessLayer;

namespace User.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserLogic userLogic;

        public UsersController(IUserLogic userLogic)
        {
            this.userLogic = userLogic;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] UserDto request)
        {
            var users = await userLogic.GetUsers(request);
            if(!users.Any())
            {
                return NotFound();
            }

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] Guid id)
        {
            var user = await userLogic.GetUser<GetUserResponse>(x=>x.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateUserCommand command)
        {
            var userId = await userLogic.SaveUser(command);

            if(userId == Guid.Empty)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetUser), new { id = userId }, userId);

        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch([FromRoute]Guid id,[FromBody]IEnumerable<Operation> patchOperations)
        {
            var jsonPatchDocument = new JsonPatchDocument();
            jsonPatchDocument.Operations.AddRange(patchOperations);

            var isUpdated = await userLogic.UpdateUser(id, jsonPatchDocument);

            if (!isUpdated)
            {
                return BadRequest();
            }

            return NoContent();
        }       
    }
}
