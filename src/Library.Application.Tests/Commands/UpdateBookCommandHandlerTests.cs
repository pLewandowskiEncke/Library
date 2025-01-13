using AutoMapper;
using FluentAssertions;
using Library.Application.Commands.CreateBook;
using Library.Application.Commands.UpdateBook;
using Library.Application.DTOs;
using Library.Domain.Entities;
using Library.Domain.Enums;
using Library.Domain.Interfaces;
using Library.Shared.Exceptions;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace Library.Application.Tests.Commands
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
        public async Task Handle_ShouldThrowInvalidBookStateException_WhenInvalidState()
        {
            // Arrange
            var book = new Book { Id = 1 };
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
        public async Task Handle_ShouldUpdateBook_WhenBookFound()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "foo" };
            var command = new UpdateBookCommand { Id = 1, Status = BookStatus.Borrowed, Title = "bar" };
            _mocker.GetMock<IUnitOfWork>()
                .Setup(u => u.BookRepository.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(book);
            var expected = new BookDTO { Id = 1 };
            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<BookDTO>(book))
                .Returns(expected);
            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map(It.IsAny<UpdateBookCommand>(), It.IsAny<Book>()))
                .Callback<UpdateBookCommand, Book>((cmd, b) =>
                {
                    b.Title = cmd.Title;
                });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mocker.GetMock<IUnitOfWork>().Verify(u => u.BeginTransaction(), Times.Once);
            _mocker.GetMock<IUnitOfWork>().Verify(u => u.BookRepository.UpdateAsync(It.Is<Book>(
                book => book.Id == 1 &&
                book.Status == BookStatus.Borrowed &&
                book.Title == "bar"
                )), Times.Once);
            _mocker.GetMock<IUnitOfWork>().Verify(u => u.Commit(), Times.Once);
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task Handle_ShouldCallTryChangeStatus_WhenBookFound()
        {
            // Arrange
            var status = BookStatus.Borrowed;
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
            book.Verify(b => b.TryChangeStatus(status), Times.Once);
        }
    }
}
