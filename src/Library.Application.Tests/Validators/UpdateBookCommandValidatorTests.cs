using FluentValidation.TestHelper;
using Library.Application.Commands.UpdateBook;
using Library.Application.Validators;
using Library.Domain.Interfaces;
using Moq;
using Xunit;

namespace Library.Application.Tests.Validators
{
    public class UpdateBookCommandValidatorTests
    {
        private readonly UpdateBookCommandValidator _validator;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        public UpdateBookCommandValidatorTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(u => u.BookRepository.IsISBNUniqueAsync(It.IsAny<string>())).ReturnsAsync(true);
            _validator = new UpdateBookCommandValidator(_unitOfWorkMock.Object);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task ShouldHaveError_WhenTitleIsNulOrEmptyAsync(string? value)
        {
            // Arrange
            var command = new UpdateBookCommand { Title = value };

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
            var command = new UpdateBookCommand { Author = value };

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
            var command = new UpdateBookCommand { ISBN = value };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.ISBN);
        }

        [Fact]
        public async Task ShouldHaveError_WhenISBNIsNotUnique()
        {
            // Arrange
            var command = new UpdateBookCommand { ISBN = string.Empty };
            _unitOfWorkMock.Setup(u => u.BookRepository.IsISBNUniqueAsync(It.IsAny<string>())).ReturnsAsync(false);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.ISBN);
        }

        [Fact]
        public async Task ShouldNotHaveError_WhenAllFieldsAreValidAsync()
        {
            // Arrange
            var command = new UpdateBookCommand
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
