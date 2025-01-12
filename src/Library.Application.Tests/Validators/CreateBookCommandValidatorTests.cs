using FluentValidation.TestHelper;
using Library.Application.Commands.CreateBook;
using Library.Application.Validators;
using Library.Domain.Interfaces;
using Moq;
using Xunit;

namespace Library.Application.Tests.Validators
{
    public class CreateBookCommandValidatorTests
    {
        private readonly CreateBookCommandValidator _validator;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        public CreateBookCommandValidatorTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(u => u.BookRepository.IsISBNUniqueAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(true);
            _validator = new CreateBookCommandValidator(_unitOfWorkMock.Object);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task ShouldHaveError_WhenTitleIsNulOrEmptyAsync(string? value)
        {
            // Arrange
            var command = new CreateBookCommand { Title = value };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Title);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task ShouldHaveError_WhenAuthorIsEmptyAsync(string? value)
        {
            // Arrange
            var command = new CreateBookCommand { Author = value };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Author);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task ShouldHaveError_WhenISBNIsEmptyAsync(string? value)
        {
            // Arrange
            var command = new CreateBookCommand { ISBN = value };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.ISBN);
        }

        [Fact]
        public async Task ShouldHaveError_WhenISBNIsNotUnique()
        {
            // Arrange
            var command = new CreateBookCommand { ISBN = string.Empty };
            _unitOfWorkMock.Setup(u => u.BookRepository.IsISBNUniqueAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(false);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.ISBN);
        }

        [Fact]
        public async Task ShouldNotHaveError_WhenAllFieldsAreValidAsync()
        {
            // Arrange
            var command = new CreateBookCommand
            {
                Title = "Valid Title",
                Author = "Valid Author",
                ISBN = "1234567890"
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.Title);
            result.ShouldNotHaveValidationErrorFor(c => c.Author);
            result.ShouldNotHaveValidationErrorFor(c => c.ISBN);
        }
    }
}
