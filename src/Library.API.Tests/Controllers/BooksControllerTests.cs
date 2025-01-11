using AutoFixture;
using FluentAssertions;
using Library.Application.Commands.CreateBook;
using Library.Application.Commands.DeleteBook;
using Library.Application.DTOs;
using Library.Application.Queries.GetBookById;
using Library.Application.Queries.GetBooks;
using Library.Controllers;
using Library.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
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

            // Set up the HttpContext for the controller
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "https";
            httpContext.Request.Host = new HostString("localhost");
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }

        [Fact]
        public async Task CreateBook_ShouldReturnsCreatedResult_WithCreatedBook()
        {
            // Arrange            
            var command = _fixture.Create<CreateBookCommand>();
            var bookDto = _fixture.Build<BookDTO>().With(x => x.Id, 1).Create();
            _mocker.GetMock<IMediator>()
                .Setup(m => m.Send(It.IsAny<CreateBookCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(bookDto);

            // Set up the UrlHelper for the controller
            var urlHelperMock = new Mock<IUrlHelper>();
            urlHelperMock.Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns($"foo");
            _controller.Url = urlHelperMock.Object;

            // Act
            var result = await _controller.CreateBook(command);

            // Assert
            result.Should().BeOfType<ActionResult<BookDTO>>();
            var returnedResult = result.Result as CreatedResult;
            returnedResult.Value.Should().Be(bookDto);
            returnedResult.Location.Should().Contain($"foo");
        }

        [Fact]
        public async Task DeleteBook_ShouldReturnNoContentResult()
        {
            // Arrange            
            var bookId = 1;
            var command = _fixture.Create<DeleteBookCommand>();
            var bookDto = _fixture.Build<BookDTO>().With(x => x.Id, 1).Create();
            _mocker.GetMock<IMediator>()
                .Setup(m => m.Send(It.Is<DeleteBookCommand>(x => x.Id == bookId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            // Act
            var result = await _controller.DeleteBook(bookId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task UpdateBook_ShouldReturnOkResult_WithUpdatedBook()
        {
            // Arrange
            var bookId = 1;
            var updateBookCommand = new UpdateBookCommand { Id = bookId, Title = "Updated Title" };
            var updatedBook = new BookDTO { Id = bookId, Title = "Updated Title" };

            _mocker.GetMock<IMediator>()
                .Setup(m => m.Send(It.Is<UpdateBookCommand>(x => x.Id == bookId && x.Title == "Updated Title"), default))
                .ReturnsAsync(updatedBook);

            // Act
            var result = await _controller.UpdateBook(bookId, updateBookCommand);

            // Assert
            result.Should().BeOfType<ActionResult<BookDTO>>();
            var returnedResult = result.Result as OkObjectResult;
            returnedResult.Value.Should().Be(updatedBook);
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
            var books = _fixture.Create<BookListDTO>();
            _mocker.GetMock<IMediator>()
                .Setup(m => m.Send(It.IsAny<GetBooksQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(books);
            var query = new GetBooksQuery();

            // Act
            var result = await _controller.GetBooks(query);

            // Assert
            result.Should().BeOfType<ActionResult<BookListDTO>>();
            var okResult = result.Result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(books);
        }
    }
}