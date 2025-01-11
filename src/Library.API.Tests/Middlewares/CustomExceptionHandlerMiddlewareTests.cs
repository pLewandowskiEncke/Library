using FluentAssertions;
using Library.API.Middlewares;
using Library.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;
using Newtonsoft.Json;
using Xunit;

namespace Library.API.Tests.Middlewares
{
    public class CustomExceptionHandlerMiddlewareTests
    {
        private readonly AutoMocker _autoMocker;
        private readonly Mock<RequestDelegate> _nextMock;
        private readonly CustomExceptionHandlerMiddleware _middleware;
        const string ContentType = "application/json; charset=utf-8";

        public CustomExceptionHandlerMiddlewareTests()
        {
            _autoMocker = new AutoMocker();
            _nextMock = new Mock<RequestDelegate>();
            _autoMocker.Use(_nextMock.Object);
            _middleware = _autoMocker.CreateInstance<CustomExceptionHandlerMiddleware>();
        }

        [Fact]
        public async Task InvokeAsync_ShouldReturnNotFound_WhenNotFoundExceptionIsThrown()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var message = "Book not found";
            context.Response.Body = new MemoryStream();
            _nextMock.Setup(next => next(It.IsAny<HttpContext>())).Throws(new NotFoundException(message));

            // Act
            await _middleware.InvokeAsync(context);

            // Assert
            context.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            context.Response.ContentType.Should().Be(ContentType);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
            var problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(responseBody);

            problemDetails.Status.Should().Be(StatusCodes.Status404NotFound);
            problemDetails.Title.Should().Be("Not Found");
            problemDetails.Detail.Should().Be(message);
        }

        [Fact]
        public async Task InvokeAsync_ShouldReturnUnprocessableEntity_WhenInvalidBookStateExceptionIsThrown()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var message = "Cannot change book state";
            context.Response.Body = new MemoryStream();
            _nextMock.Setup(next => next(It.IsAny<HttpContext>())).Throws(new InvalidBookStateException(message));

            // Act
            await _middleware.InvokeAsync(context);

            // Assert
            context.Response.StatusCode.Should().Be(StatusCodes.Status422UnprocessableEntity);
            context.Response.ContentType.Should().Be(ContentType);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
            var problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(responseBody);

            problemDetails.Status.Should().Be(StatusCodes.Status422UnprocessableEntity);
            problemDetails.Title.Should().Be("Invalid Book State");
            problemDetails.Detail.Should().Be(message);
        }

        [Fact]
        public async Task InvokeAsync_ShouldReturnInternalServerError_WhenGenericExceptionIsThrown()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var message = "Something happened";
            context.Response.Body = new MemoryStream();
            _nextMock.Setup(next => next(It.IsAny<HttpContext>())).Throws(new Exception(message));

            // Act
            await _middleware.InvokeAsync(context);

            // Assert
            context.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            context.Response.ContentType.Should().Be(ContentType);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
            var problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(responseBody);

            problemDetails.Status.Should().Be(StatusCodes.Status500InternalServerError);
            problemDetails.Title.Should().Be("Internal Server Error");
            problemDetails.Detail.Should().Be(message);
        }
    }
}
