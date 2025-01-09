using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Library.Application.Commands.CreateBook;
using Library.Application.DTOs;
using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace Library.Application.Tests.Commands
{
    public class CreateBookCommandHandlerTests
    {
        private readonly Fixture _fixture;
        private readonly AutoMocker _mocker;
        private readonly CreateBookCommandHandler _handler;

        public CreateBookCommandHandlerTests()
        {
            _fixture = new Fixture();
            _mocker = new AutoMocker();
            _handler = _mocker.CreateInstance<CreateBookCommandHandler>();
        }

        [Fact]
        public async Task Handle_ShouldReturnsBookDTO_WhenAddingBookWasSuccesful()
        {
            // Arrange
            var command = _fixture.Create<CreateBookCommand>();
            var book = _fixture.Create<Book>();
            var bookDTO = _fixture.Create<BookDTO>();

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<Book>(command)).Returns(book);
            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<BookDTO>(book)).Returns(bookDTO);
            _mocker.GetMock<IUnitOfWork>()
                .Setup(u => u.BookRepository.AddAsync(It.IsAny<Book>())).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(bookDTO);
        }

        [Fact]
        public async Task Handle_ShouldAddBookInTransaction()
        {
            // Arrange
            var command = _fixture.Create<CreateBookCommand>();
            var book = _fixture.Create<Book>();
            var bookDTO = _fixture.Create<BookDTO>();

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<Book>(command)).Returns(book);
            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<BookDTO>(book)).Returns(bookDTO);
            _mocker.GetMock<IUnitOfWork>()
                .Setup(u => u.BookRepository.AddAsync(It.IsAny<Book>())).Returns(Task.CompletedTask);


            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            var sequence = new MockSequence();
            var unitOfWorkMock = _mocker.GetMock<IUnitOfWork>();
            unitOfWorkMock.InSequence(sequence).Setup(u => u.BeginTransaction());
            unitOfWorkMock.InSequence(sequence).Setup(u => u.BookRepository.AddAsync(book));
            unitOfWorkMock.InSequence(sequence).Setup(u => u.Commit());
        }
    }
}