using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NrAcademyBL.Abstractions; // IBaseException və BaseException üçün

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
            // Default dəyərlər (Gözlənilməz sistem xətaları üçün)
            var statusCode = StatusCodes.Status500InternalServerError;
            var errorCode = "INTERNAL_SERVER_ERROR";
            var message = ex.Message;

            // Bizim xüsusi xətalarımızı (BaseException-dan törəyənləri) tutur
            if (ex is IBaseException customEx)
            {
                statusCode = customEx.StatusCode;
                errorCode = customEx.ErrorCode;
            }
            // Əgər framework-ün daxili EntityNotFoundException xətasını da tutmaq istəyirsənsə
            else if (ex.GetType().Name == "EntityNotFoundException")
            {
                statusCode = StatusCodes.Status404NotFound;
                errorCode = "ENTITY_NOT_FOUND";
            }

            var result = JsonConvert.SerializeObject(new
            {
                success = false,
                errorCode,
                message
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            return context.Response.WriteAsync(result);
        }
    }
}