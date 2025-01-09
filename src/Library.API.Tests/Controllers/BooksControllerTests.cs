using AutoFixture;
using FluentAssertions;
using Library.Application.Commands.CreateBook;
using Library.Application.DTOs;
using Library.Application.Queries.GetBookById;
using Library.Application.Queries.GetBooks;
using Library.Controllers;
using Library.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace Library.API.Tests.Controllers
{
    public class BooksControllerTests
    {
        private readonly Fixture _fixture;
        private readonly AutoMocker _mocker;
        private readonly BooksController _controller;

        public BooksControllerTests()
        {
            _fixture = new Fixture();
            _mocker = new AutoMocker();
            _controller = _mocker.CreateInstance<BooksController>();
        }

        [Fact]
        public async Task CreateBook_ShouldReturnsOkResult()
        {
            // Arrange
            
            var command = _fixture.Create<CreateBookCommand>();
            var bookDto = _fixture.Create<BookDTO>();
            _mocker.GetMock<IMediator>()
                .Setup(m => m.Send(It.IsAny<CreateBookCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(bookDto);

            // Act
            var result = await _controller.CreateBook(command);

            // Assert
            result.Should().BeOfType<ActionResult<BookDTO>>();
            var okResult = result.Result as OkObjectResult;
            okResult.Value.Should().Be(bookDto);
        }
        [Fact]
        public async Task GetBookById_ShouldReturnOkResult()
        {
            // Arrange
            var bookId = _fixture.Create<int>();
            var bookDto = _fixture.Create<BookDTO>();
            _mocker.GetMock<IMediator>()
                .Setup(m => m.Send(It.IsAny<GetBookByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(bookDto);

            // Act
            var result = await _controller.GetBookById(bookId);

            // Assert
            result.Should().BeOfType<ActionResult<BookDTO>>();
            var okResult = result.Result as OkObjectResult;
            okResult.Value.Should().Be(bookDto);
        }

        [Fact]
        public async Task GetBooks_ShouldReturnOkResult()
        {
            // Arrange
            var books = _fixture.CreateMany<BookDTO>();
            _mocker.GetMock<IMediator>()
                .Setup(m => m.Send(It.IsAny<GetBooksQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(books);

            // Act
            var result = await _controller.GetBooks();

            // Assert
            result.Should().BeOfType<ActionResult<IEnumerable<BookDTO>>>();
            var okResult = result.Result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(books);
        }
    }
}