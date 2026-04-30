using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Abp.Domain.Entities;
namespace NrAcademyBL.Exceptions
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private Task HandleException(HttpContext context, Exception ex)
        {

            var code = StatusCodes.Status500InternalServerError;
            var errors = new List<string> { ex.Message };

            switch (ex)
            {
                case EntityNotFoundException:
                    code = StatusCodes.Status404NotFound;
                    break;
            }

            var result = JsonConvert.SerializeObject(new { errors });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = code;

            return context.Response.WriteAsync(result);
        }

    }
}
