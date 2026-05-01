// NrAcademyBL/Exceptions/GlobalExceptionHandlerMiddleware.cs
using Abp.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NrAcademyBL.Exceptions.Base;

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
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var statusCode = StatusCodes.Status500InternalServerError;
            var errorCode = "INTERNAL_SERVER_ERROR";
            var message = ex.Message;

            if (ex is NrAcademyException nrEx)
            {
                statusCode = nrEx.StatusCode;
                errorCode = nrEx.ErrorCode;
            }
            else if (ex is EntityNotFoundException)
            {
                statusCode = StatusCodes.Status404NotFound;
                errorCode = "ENTITY_NOT_FOUND";
            }

            var result = JsonConvert.SerializeObject(new
            {
                success = false,
                errorCode,
                message,
                // statusCode = statusCode   // frontend istəsə əlavə edə bilərsən
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            return context.Response.WriteAsync(result);
        }
    }
}