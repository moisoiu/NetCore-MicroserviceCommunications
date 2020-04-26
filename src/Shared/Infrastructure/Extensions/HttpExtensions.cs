using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using DTO.Configuration;
using Microsoft.AspNetCore.Authentication;

namespace Infrastructure.Extensions
{
    public static class HttpExtensions
    {
        public static async Task<T> ConvertStringContentAsJson<T>(this HttpContent content)
        {
            var contentResult = await content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(contentResult);
        }

        public static async Task<IActionResult> UpdateActionResult(this HttpResponseMessage responseMessage)
        {
            var content = await responseMessage.Content.ReadAsStringAsync();
            switch (responseMessage.StatusCode)
            {
                case HttpStatusCode.Conflict:
                    return new ConflictObjectResult(content);

                default:
                    return new BadRequestObjectResult(content);
            }
        }

        public static async Task<IActionResult> GetActionResult(this HttpResponseMessage responseMessage)
        {
            var content = await responseMessage.Content.ReadAsStringAsync();

            switch (responseMessage.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return new BadRequestObjectResult(content);

                default:
                    return new NotFoundResult();
            }
        }
    }
}
