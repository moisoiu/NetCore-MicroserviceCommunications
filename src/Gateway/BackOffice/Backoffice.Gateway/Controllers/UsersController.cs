using Backoffice.Gateway.Communications.Refit;
using DTO.User;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using DTO.Configuration;
using Backoffice.Gateway.Models.User;

namespace Backoffice.Gateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserApi userApi;
        private readonly IConfiguration configuration;
        private readonly Settings settings;

        public UsersController(
            IUserApi userApi,
            IConfiguration configuration,
            Settings settings)
        {
            this.userApi = userApi;
            this.configuration = configuration;
            this.settings = settings;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateUserRequest request)
        {
            var loginPayload = new Dictionary<string, object>();
            loginPayload.AuthenticationPayload(request, settings);

            var response = await userApi.AuthenticateUser(loginPayload);
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            return Ok(await response.Content.ReadAsStringAsync());
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshLogin(string refreshToken)
        {
            if(string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest();
            }

            var refreshTokenPayload = new Dictionary<string, object>();
            refreshTokenPayload.RefreshTokenPayload(refreshToken, settings);

            var response = await userApi.RefreshTokenUser(refreshTokenPayload);
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            return Ok(await response.Content.ReadAsStringAsync());
        }

        [HttpPost("revoke-refresh-token")]
        public async Task<IActionResult> RevokeRefreshToken(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest();
            }

            var loginPayload = new Dictionary<string, object>();
            loginPayload.RevokeRefreshTokenPayload(refreshToken, settings);

            var response = await userApi.RevokeTokenUser(loginPayload);
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            return Ok(await response.Content.ReadAsStringAsync());
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUsers([FromQuery]GetUsersRequest request)
        {
            var usersRequest = await userApi.GetUsers(request);

            if (!usersRequest.IsSuccessStatusCode)
            {
                return await usersRequest.GetActionResult();
            }

            var users = await usersRequest.Content.ConvertStringContentAsJson<IEnumerable<GetUserResponse>>();


            return Ok(users);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserById([FromRoute]Guid id)
        {
            var userRequest = await userApi.GetUser(id);

            if (!userRequest.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var user = await userRequest.Content.ConvertStringContentAsJson<GetUserResponse>();

            return Ok(user);
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Post([FromBody]CreateUserRequest command)
        {
            var userCreateResponse = await userApi.SaveUser(command);

            var userCreateContent = await userCreateResponse.Content.ConvertStringContentAsJson<string>();

            if (!userCreateResponse.IsSuccessStatusCode)
            {
                return BadRequest(userCreateContent);
            }

            return CreatedAtAction(nameof(GetUserById), new { id = userCreateContent }, userCreateContent);
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Patch([FromRoute]Guid id, [FromBody]UpdateUserRequest patchUser)
        {
            var patch = JsonPatchDocumentExtensions.CreatePatch(patchUser.Original, patchUser.Changed);

            var patchResponse = await userApi.PatchUser(id, patch.Operations);

            if (!patchResponse.IsSuccessStatusCode)
            {
                return await patchResponse.UpdateActionResult();
            }

            return NoContent(); // Because theoretically the user on frontend already has the data changed, no point in returning a new data 

        }
    }
}
