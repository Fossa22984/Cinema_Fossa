using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using OnlineCinema_Core.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Online_Cinema_UI.Filters
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
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

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            Log.Current.Error(ex);
            var code = HttpStatusCode.InternalServerError;

            var result = JsonConvert.SerializeObject(ResponseModelBuild(GetErrors(ex)));
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }

        private static string[] GetErrors(Exception ex)
        {
            List<string> errors = new List<string>();
            errors.Add(ex.Message);

            var error = ex;

            while (error.InnerException != null && error != error.InnerException)
            {
                error = error.InnerException;
                errors.Add(error.Message);
            }

            return errors.ToArray();
        }

        private static ErrorResponceApiModel ResponseModelBuild(params string[] errors)
        {
            ErrorResponceApiModel result = new ErrorResponceApiModel()
            {
                Errors = errors.ToList()
            };
            return result;
        }

        public class ErrorResponceApiModel
        {
            [JsonProperty("errors")] public List<string> Errors { get; set; }
        }
    }
}
