using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;
using Serilog.Context;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Infrastructure.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger = Log.ForContext<ErrorHandlingMiddleware>();

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError;

          
            switch (exception)
            {
                case DbUpdateConcurrencyException ex:
                    statusCode = HttpStatusCode.Conflict;
                    logger.Warning(ex, $"Warning: Exception of type {nameof(DbUpdateConcurrencyException)}");
                    break;

                default:
                    break;
            }

            if(logger.IsEnabled(Serilog.Events.LogEventLevel.Verbose))
            {
                string serverRequest = string.Empty;
                if (context.Request.Method != "GET")
                {
                    var stream = new StreamReader(context.Request.Body);
                    stream.BaseStream.Seek(0, SeekOrigin.Begin);
                    serverRequest = stream.ReadToEnd();
                }

                LogContext.PushProperty("HTTPMethod", context.Request.Method);
                LogContext.PushProperty("RequestBody", context.Request.Path.HasValue && context.Request.Path.Value.Contains("login") ? "Login Exception" : serverRequest);
                LogContext.PushProperty("RequestQuery", context.Request.QueryString.HasValue ? context.Request.QueryString.Value : string.Empty);                

            }

            context.Response.Clear();
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsync(JsonConvert.SerializeObject(exception));
        }
    }
}
