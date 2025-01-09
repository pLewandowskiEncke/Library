//using AutoMapper;
//using FluentAssertions;
//using Library.Application.Commands.CreateBook;
//using Library.Application.DTOs;
//using Library.Domain.Entities;
//using Moq;
//using Xunit;

//namespace Library.Application.Tests.Commands
//{
//    public class CreateBookCommandHandlerTests
//    {
//        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
//        private readonly Mock<IMapper> _mapperMock;
//        private readonly CreateBookCommandHandler _handler;

//        public CreateBookCommandHandlerTests()
//        {
//            _unitOfWorkMock = new Mock<IUnitOfWork>();
//            _mapperMock = new Mock<IMapper>();
//            _handler = new CreateBookCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);
//        }

//        [Fact]
//        public async Task Handle_ValidCommand_ReturnsBookDTO()
//        {
//            // Arrange
//            var command = new CreateBookCommand { Title = "Test Book", Author = "Test Author", ISBN = "1234567890", Status = BookStatus.OnTheShelf };
//            var book = new Book { Id = 1, Title = "Test Book", Author = "Test Author", ISBN = "1234567890", Status = BookStatus.OnTheShelf };
//            var bookDTO = new BookDTO { Id = 1, Title = "Test Book", Author = "Test Author", ISBN = "1234567890", Status = BookStatus.OnTheShelf };

//            _mapperMock.Setup(m => m.Map<Book>(command)).Returns(book);
//            _mapperMock.Setup(m => m.Map<BookDTO>(book)).Returns(bookDTO);
//            _unitOfWorkMock.Setup(u => u.BookRepository.AddAsync(It.IsAny<Book>())).Returns(Task.CompletedTask);
//            _unitOfWorkMock.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);

//            // Act
//            var result = await _handler.Handle(command, CancellationToken.None);

//            // Assert
//            result.Should().BeEquivalentTo(bookDTO);
//        }
//    }
//}