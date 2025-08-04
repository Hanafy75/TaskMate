using FluentValidation;
using System.Net;
using System.Text.Json;
using TaskMate.Application.Exceptions;
using TaskMate.WebAPI.Responses;

namespace TaskMate.WebAPI.Middlewares
{
    public class ErrorHandlingMiddleware 
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                _logger.LogError("An unhandled exception occurred.");
                await HandleExceptionAsync(context,ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode statusCode;
            ApiResponse<object> response;
                 
            switch(exception)
            {

                case ValidationException ex:
                    statusCode = HttpStatusCode.BadRequest;
                    response = ApiResponse<object>.Fail("Invalid Input", ex.Errors.Select(e=>e.ErrorMessage));
                    break;
                case UserCreationException ex:
                    statusCode = HttpStatusCode.BadRequest;
                    response = ApiResponse<object>.Fail(ex.Message, ex.Errors);
                    break;

                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    response = ApiResponse<object>.Fail("An internal server error has occurred. Please contact support.");
                    break;
            }

            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            var result = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(result);
        }
    }
}
