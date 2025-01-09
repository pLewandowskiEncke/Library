using FluentAssertions;
using Library.Application.Commands.CreateBook;
using Library.Application.DTOs;
using Library.Controllers;
using Library.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Library.API.Tests.Controllers
{
    public class BooksControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly BooksController _controller;

        public BooksControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new BooksController(_mediatorMock.Object);
        }

        [Fact]
        public async Task CreateBook_ReturnsOkResult()
        {
            // Arrange
            var command = new CreateBookCommand { Title = "Test Book", Author = "Test Author", ISBN = "1234567890", Status = BookStatus.OnTheShelf };
            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateBookCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new BookDTO());

            // Act
            var result = await _controller.CreateBook(command);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}