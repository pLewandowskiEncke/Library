using AutoMapper;
using FluentAssertions;
using Library.Application.Commands.CreateBook;
using Library.Application.Commands.PatchBook;
using Library.Application.Commands.UpdateBook;
using Library.Application.DTOs;
using Library.Application.Mappings;
using Library.Domain.Entities;
using Library.Domain.Enums;
using Xunit;

namespace Library.Application.Tests.Mappings
{
    public class AutomapperBookProfileTests
    {
        private readonly IMapper _mapper;

        public AutomapperBookProfileTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutomapperBookProfile>();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void AutomapperBookProfile_ConfigurationIsValid()
        {
            // Arrange & Act
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutomapperBookProfile>();
            });

            // Assert
            config.AssertConfigurationIsValid();
        }

        [Fact]
        public void Map_CreateBookCommand_To_Book_ShouldReturnExpectedBook()
        {
            // Arrange
            var command = new CreateBookCommand
            {
                Title = "Test Title",
                Author = "Test Author",
                ISBN = "1234567890"
            };

            // Act
            var book = _mapper.Map<Book>(command);

            // Assert
            book.Title.Should().Be(command.Title);
            book.Author.Should().Be(command.Author);
            book.ISBN.Should().Be(command.ISBN);
            book.Id.Should().Be(0); // Id should be ignored
        }

        [Fact]
        public void Map_UpdateBookCommand_To_Book_ShouldReturnExpectedBook()
        {
            // Arrange
            var command = new UpdateBookCommand
            {
                Title = "Updated Title",
                Author = "Updated Author",
                ISBN = "0987654321"
            };

            // Act
            var book = _mapper.Map<Book>(command);

            // Assert
            book.Title.Should().Be(command.Title);
            book.Author.Should().Be(command.Author);
            book.ISBN.Should().Be(command.ISBN);
            book.Id.Should().Be(0); // Id should be ignored
        }

        [Fact]
        public void Map_PatchBookCommand_To_Book_ShouldReturnExpectedBook()
        {
            // Arrange
            var command = new PatchBookCommand
            {
                Title = "Patched Title",
                Author = "Patched Author",
                ISBN = "1122334455",
                Status = BookStatus.OnTheShelf
            };

            // Act
            var book = _mapper.Map<Book>(command);

            // Assert
            book.Title.Should().Be(command.Title);
            book.Author.Should().Be(command.Author);
            book.ISBN.Should().Be(command.ISBN);
            book.Status.Should().Be(command.Status);
            book.Id.Should().Be(0); // Id should be ignored
        }

        [Fact]
        public void Map_PatchBookCommand_To_Book_ShouldNotMapNullProperties()
        {
            // Arrange
            var command = new PatchBookCommand
            {
                Status = BookStatus.Borrowed
            };
            var book = new Book
            {
                Title = "Test Title",
                Author = "Test Author",
                ISBN = "1234567890",
            };

            // Act
            _mapper.Map(command, book);

            // Assert
            book.Title.Should().Be("Test Title");
            book.Author.Should().Be("Test Author");
            book.ISBN.Should().Be("1234567890");
            book.Status.Should().Be(BookStatus.OnTheShelf); // Status should not be updated
            book.Id.Should().Be(0); // Id should be ignored
        }

        [Fact]
        public void Map_Book_To_BookDTO_ShouldReturnExpectedBookDTO()
        {
            // Arrange
            var book = new Book
            {
                Id = 1,
                Title = "Test Title",
                Author = "Test Author",
                ISBN = "1234567890"
            };

            // Act
            var bookDTO = _mapper.Map<BookDTO>(book);

            // Assert
            bookDTO.Id.Should().Be(book.Id);
            bookDTO.Title.Should().Be(book.Title);
            bookDTO.Author.Should().Be(book.Author);
            bookDTO.ISBN.Should().Be(book.ISBN);
            bookDTO.Status.Should().Be(book.Status);
        }
    }
}
