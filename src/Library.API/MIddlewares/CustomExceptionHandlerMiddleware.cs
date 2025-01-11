using Library.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Library.API.Middlewares
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionHandlerMiddleware(RequestDelegate next)
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

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            int statusCode;
            string title;

            if (exception is NotFoundException)
            {
                statusCode = StatusCodes.Status404NotFound;
                title = "Not Found";
            }
            else if (exception is InvalidBookStateException)
            {
                statusCode = StatusCodes.Status422UnprocessableEntity;
                title = "Invalid Book State";
            }
            else
            {
                statusCode = StatusCodes.Status500InternalServerError;
                title = "Internal Server Error";
            }

            context.Response.StatusCode = statusCode;
            var problem = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = exception.Message
            };
            return context.Response.WriteAsJsonAsync(problem);
        }
    }
}