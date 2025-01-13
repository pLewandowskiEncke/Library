using AutoMapper;
using FluentAssertions;
using Library.Application.Commands.PatchBook;
using Library.Application.Mappings;
using Library.Domain.Entities;
using Library.Domain.Enums;
using Library.Domain.Interfaces;
using Library.Shared.Exceptions;
using Moq;
using Xunit;

namespace Library.Application.Tests.Commands
{
    public class PatchBookCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IMapper _mapper;
        private readonly PatchBookCommandHandler _handler;

        public PatchBookCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutomapperBookProfile>();
            });
            _mapper = config.CreateMapper();

            _handler = new PatchBookCommandHandler(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldUpdatesBook_WhenValidCommandProvided()
        {
            // Arrange
            var book = new Book
            {
                Id = 1,
                Title = "Original Title",
                Author = "Original Author",
                ISBN = "1234567890"
            };

            var command = new PatchBookCommand
            {
                Id = 1,
                Title = "Updated Title",
                Author = "Updated Author",
                ISBN = "0987654321",
                Status = BookStatus.Borrowed
            };

            _unitOfWorkMock.Setup(u => u.BookRepository.GetByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync(book);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(command);

            _unitOfWorkMock.Verify(u => u.BeginTransaction(), Times.Once);
            _unitOfWorkMock.Verify(u => u.BookRepository.UpdateAsync(It.IsAny<Book>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldChangeStatus_WhenValidCommandProvided()
        {
            // Arrange
            var book = new Book
            {
                Id = 1,
                Title = "Original Title",
                Author = "Original Author",
                ISBN = "1234567890"
            };

            var command = new PatchBookCommand
            {
                Id = 1,
                Status = BookStatus.Borrowed
            };

            _unitOfWorkMock.Setup(u => u.BookRepository.GetByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync(book);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(command.Status);

            _unitOfWorkMock.Verify(u => u.BookRepository.UpdateAsync(It.IsAny<Book>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldNotUpdateBookValues_WhenNewValuesAreNull()
        {
            // Arrange
            var book = new Book
            {
                Id = 1,
                Title = "Original Title",
                Author = "Original Author",
                ISBN = "1234567890"
            };

            var command = new PatchBookCommand
            {
                Id = 1,
            };

            _unitOfWorkMock.Setup(u => u.BookRepository.GetByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync(book);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(book);

            _unitOfWorkMock.Verify(u => u.BookRepository.UpdateAsync(It.IsAny<Book>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }
    }
}
