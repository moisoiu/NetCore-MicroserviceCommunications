using DTO.User;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Backoffice.Gateway.Communications.Refit
{
    public interface IUserApi
    {
        [Get("/api/users/{id}")]
        Task<HttpResponseMessage> GetUser(Guid id);

        [Get("/api/users")]
        Task<HttpResponseMessage> GetUsers([Query]GetUsersRequest request);

        [Post("/api/users")]
        Task<HttpResponseMessage> SaveUser([Body]CreateUserRequest command);

        [Patch("/api/users/{id}")]
        Task<HttpResponseMessage> PatchUser(Guid id, [Body]IEnumerable<Operation> patchUser);

        [Post("/connect/token")]
        Task<HttpResponseMessage> AuthenticateUser([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> loginPayload);

        [Post("/connect/token")]
        Task<HttpResponseMessage> RefreshTokenUser([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> refreshPayload);

        [Post("/connect/revocation")]
        Task<HttpResponseMessage> RevokeTokenUser([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> refreshPayload);
    }
}
