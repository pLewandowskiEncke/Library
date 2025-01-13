using AutoMapper;
using FluentAssertions;
using Library.Application.DTOs;
using Library.Application.Queries.GetBooks;
using Library.Domain.Entities;
using Library.Domain.Enums;
using Library.Domain.Interfaces;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace Library.Application.Tests.Queries
{
    public class GetBooksQueryHandlerTests
    {
        private readonly AutoMocker _mocker;
        private readonly GetBooksQueryHandler _handler;
        const int PageNumber = 1;
        const int PageSize = 10;
        const string SortBy = "Title";
        const bool Ascending = true;

        public GetBooksQueryHandlerTests()
        {
            _mocker = new AutoMocker();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Book, BookDTO>();
            });
            var mapper = config.CreateMapper();
            _mocker.Use(mapper);

            _handler = _mocker.CreateInstance<GetBooksQueryHandler>();
        }

        [Fact]
        public async Task Handle_ShouldReturnBookListDTO_WhenBooksExist()
        {
            // Arrange

            var books = new List<Book>
                {
                    new() { Id = 1, Title = "Test Book 1" },
                    new() { Id = 2, Title = "Test Book 2" }
                };

            _mocker.GetMock<IUnitOfWork>()
                .Setup(uow => uow.BookRepository.GetBooks(1, 10, "Title", true))
                .ReturnsAsync(books);
            var expected = new BookListDTO
            {
                PageNumber = PageNumber,
                PageSize = PageSize,
                SortBy = SortBy,
                Ascending = Ascending,
                Books =
                [
                    new() { Id = 1, Title = "Test Book 1", Status = BookStatus.OnTheShelf },
                    new() { Id = 2, Title = "Test Book 2", Status = BookStatus.OnTheShelf }
                ]
            };


            var query = new GetBooksQuery { PageNumber = PageNumber, PageSize = PageSize, SortBy = SortBy, Ascending = Ascending };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyBookListDTO_WhenNoBooksExist()
        {
            // Arrange
            var books = new List<Book>();

            _mocker.GetMock<IUnitOfWork>()
                .Setup(uow => uow.BookRepository.GetBooks(1, 10, "Title", true))
                .ReturnsAsync(books);

            var expected = new BookListDTO
            {
                PageNumber = PageNumber,
                PageSize = PageSize,
                SortBy = SortBy,
                Ascending = Ascending,
                Books = []
            };

            var query = new GetBooksQuery { PageNumber = PageNumber, PageSize = PageSize, SortBy = SortBy, Ascending = Ascending };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);
        }
    }
}
