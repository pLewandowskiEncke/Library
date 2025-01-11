using AutoMapper;
using FluentAssertions;
using Library.Application.Commands.CreateBook;
using Library.Application.DTOs;
using Library.Domain.Entities;
using Library.Domain.Enums;
using Library.Domain.Interfaces;
using Library.Shared.Exceptions;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace Library.Application.Tests.Commands.UpdateBook
{
    public class UpdateBookCommandHandlerTests
    {
        private readonly AutoMocker _mocker;
        private readonly UpdateBookCommandHandler _handler;

        public UpdateBookCommandHandlerTests()
        {
            _mocker = new AutoMocker();
            _handler = _mocker.CreateInstance<UpdateBookCommandHandler>();
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenBookNotFound()
        {
            // Arrange
            var command = new UpdateBookCommand { Id = 1, Status = BookStatus.Borrowed };
            _mocker.GetMock<IUnitOfWork>()
                .Setup(u => u.BookRepository.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Book?)null);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Book not found");
        }

        [Fact]
        public async Task Handle_ShouldThrowInvalidBookStateException_WhenInvalidState()
        {
            // Arrange
            var book = new CustomBook(BookStatus.Borrowed) { Id = 1 };
            var command = new UpdateBookCommand { Id = 1, Status = (BookStatus)999 };
            _mocker.GetMock<IUnitOfWork>()
                .Setup(u => u.BookRepository.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(book);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidBookStateException>()
                .WithMessage("Invalid state");
        }

        [Fact]
        public async Task Handle_ShouldUpdateBookState_WhenValidState()
        {
            // Arrange
            var book = new CustomBook(BookStatus.Borrowed) { Id = 1 };
            var command = new UpdateBookCommand { Id = 1, Status = BookStatus.Returned };
            _mocker.GetMock<IUnitOfWork>()
                .Setup(u => u.BookRepository.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(book);
            var expected = new BookDTO { Id = 1 };
            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<BookDTO>(book))
                .Returns(expected);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mocker.GetMock<IUnitOfWork>().Verify(u => u.BeginTransaction(), Times.Once);
            _mocker.GetMock<IUnitOfWork>().Verify(u => u.BookRepository.UpdateAsync(It.Is<Book>(
                book => book.Id == 1 &&
                book.Status == BookStatus.Returned
                )), Times.Once);
            _mocker.GetMock<IUnitOfWork>().Verify(u => u.Commit(), Times.Once);
            result.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(BookStatus.OnTheShelf)]
        [InlineData(BookStatus.Borrowed)]
        [InlineData(BookStatus.Returned)]
        [InlineData(BookStatus.Damaged)]
        public async Task Handle_ShouldCallCorrectStateMethod_WhenValidState(BookStatus status)
        {
            // Arrange
            var book = new Mock<Book>();
            var command = new UpdateBookCommand { Id = 1, Status = status };
            _mocker.GetMock<IUnitOfWork>().
                Setup(u => u.BookRepository.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(book.Object);
            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<BookDTO>(It.IsAny<Book>()))
                .Returns(new BookDTO());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            switch (status)
            {
                case BookStatus.OnTheShelf:
                    book.Verify(b => b.PlaceOnShelf(), Times.Once);
                    break;
                case BookStatus.Borrowed:
                    book.Verify(b => b.Borrow(), Times.Once);
                    break;
                case BookStatus.Returned:
                    book.Verify(b => b.Return(), Times.Once);
                    break;
                case BookStatus.Damaged:
                    book.Verify(b => b.MarkAsDamaged(), Times.Once);
                    break;
            }
        }
    }

    internal class CustomBook : Book
    {
        public CustomBook(BookStatus status)
        {
            Status = status;
        }
    }
}
