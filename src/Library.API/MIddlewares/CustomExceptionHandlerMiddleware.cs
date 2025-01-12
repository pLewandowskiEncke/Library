using FluentValidation;
using Library.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;

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
            ProblemDetails problem;

            if (exception is NotFoundException)
            {
                problem = new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Not Found",
                    Detail = exception.Message
                };
            }
            else if (exception is InvalidBookStateException)
            {
                problem = new ProblemDetails
                {
                    Status = StatusCodes.Status422UnprocessableEntity,
                    Title = "Invalid Book State",
                    Detail = exception.Message
                };
            }
            else if (exception is InvalidBookOperationException)
            {
                problem = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Invalid Book Operation",
                    Detail = exception.Message
                };
            } 
            else if (exception is ValidationException validationExeption) 
            {
                problem = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Type = "ValidationFailure",
                    Title = "Validation error",
                    Detail = "One or more validation errors has occurred"
                };

                if (validationExeption.Errors is not null)
                {
                    problem.Extensions["errors"] = validationExeption.Errors;
                }                           
            }

            else
            {
                problem = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Internal Server Error",
                    Detail = "Something went wrong"
                };
            }

            context.Response.StatusCode = problem.Status.Value;
            return context.Response.WriteAsJsonAsync(problem);
        }
    }
}