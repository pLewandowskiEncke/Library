using FluentAssertions;
using Library.Application.Commands.DeleteBook;
using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Library.Shared.Exceptions;
using MediatR;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace Library.Application.Tests.Commands
{
    public class DeleteBookCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly AutoMocker _mocker;
        private readonly DeleteBookCommandHandler _handler;

        public DeleteBookCommandHandlerTests()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _mocker = new AutoMocker();
            _mocker.Use(_unitOfWork.Object);
            _handler = _mocker.CreateInstance<DeleteBookCommandHandler>();
        }

        [Fact]
        public async Task Handle_ShouldDeleteBook_WhenBookExists()
        {
            // Arrange
            var bookId = 1;
            var book = new Book { Id = bookId };
            _unitOfWork.Setup(u => u.BookRepository.GetByIdAsync(bookId)).ReturnsAsync(book);
            _unitOfWork.Setup(u => u.BookRepository.DeleteAsync(book)).Returns(Task.CompletedTask);
            _unitOfWork.Setup(u => u.BeginTransaction());
            _unitOfWork.Setup(u => u.Commit());

            var command = new DeleteBookCommand { Id = bookId };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
            _unitOfWork.Verify(u => u.BookRepository.GetByIdAsync(bookId), Times.Once);
            _unitOfWork.Verify(u => u.BookRepository.DeleteAsync(book), Times.Once);
            _unitOfWork.Verify(u => u.BeginTransaction(), Times.Once);
            _unitOfWork.Verify(u => u.Commit(), Times.Once);
        }
    }
}
