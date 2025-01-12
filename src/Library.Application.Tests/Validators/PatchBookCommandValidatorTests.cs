using FluentValidation.TestHelper;
using Library.Application.Commands.PatchBook;
using Library.Application.Validators;
using Library.Domain.Interfaces;
using Moq;
using Xunit;

namespace Library.Application.Tests.Validators
{
    public class PatchBookCommandValidatorTests
    {
        private readonly PatchBookCommandValidator _validator;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        public PatchBookCommandValidatorTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(u => u.BookRepository.IsISBNUniqueAsync(It.IsAny<string>())).ReturnsAsync(true);
            _validator = new PatchBookCommandValidator(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task ShouldHaveError_WhenTitleIsEmpty()
        {
            // Arrange
            var command = new PatchBookCommand { Title = string.Empty };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Title);
        }

        [Fact]
        public async Task ShouldNotHaveError_WhenTitleIsNull()
        {
            // Arrange
            var command = new PatchBookCommand { Title = null };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.Title);
        }

        [Fact]
        public async Task ShouldHaveError_WhenAuthorIsEmpty()
        {
            // Arrange
            var command = new PatchBookCommand { Author = string.Empty };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Author);
        }

        [Fact]
        public async Task ShouldNotHaveError_WhenAuthorIsNull()
        {
            // Arrange
            var command = new PatchBookCommand { Author = null };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.Author);
        }

        [Fact]
        public async Task ShouldHaveError_WhenISBNIsEmpty()
        {
            // Arrange
            var command = new PatchBookCommand { ISBN = string.Empty };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.ISBN);
        }

        [Fact]
        public async Task ShouldNotHaveError_WhenISBNIsNull()
        {
            // Arrange
            var command = new PatchBookCommand { ISBN = null };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.ISBN);
        }

        [Fact]
        public async Task ShouldHaveError_WhenISBNIsNotUnique()
        {
            // Arrange
            var command = new PatchBookCommand { ISBN = string.Empty };
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
            var command = new PatchBookCommand
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
