using AutoMapper;
using FluentAssertions;
using Library.Application.DTOs;
using Library.Application.Queries.GetBookById;
using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Library.Shared.Exceptions;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace Library.Application.Tests.Queries
{
    public class GetBookByIdQueryHandlerTests
    {
        private readonly AutoMocker _mocker;
        private readonly GetBookByIdQueryHandler _handler;

        public GetBookByIdQueryHandlerTests()
        {
            _mocker = new AutoMocker();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Book, BookDTO>();
            });
            var mapper = config.CreateMapper();
            _mocker.Use(mapper);

            _handler = _mocker.CreateInstance<GetBookByIdQueryHandler>();
        }

        [Fact]
        public async Task Handle_ShouldReturnBookDTO_WhenBookExists()
        {
            // Arrange
            var bookId = 1;
            var book = new Book
            {
                Id = bookId,
                Title = "Test Title",
                Author = "Test Author",
                ISBN = "1234567890",
            };

            _mocker.GetMock<IUnitOfWork>()
                .Setup(uow => uow.BookRepository.GetByIdAsync(bookId))
                .ReturnsAsync(book);
            var expected = new BookDTO
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                ISBN = book.ISBN,
                Status = book.Status
            };

            var query = new GetBookByIdQuery(bookId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenBookDoesNotExist()
        {
            // Arrange
            var bookId = 1;

            _mocker.GetMock<IUnitOfWork>()
                .Setup(uow => uow.BookRepository.GetByIdAsync(bookId))
                .ReturnsAsync(null as Book);

            var query = new GetBookByIdQuery(bookId);

            // Act
            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Book not found");
        }
    }
}
