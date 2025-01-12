using FluentAssertions;
using FluentValidation;
using Library.Application.Behaviors;
using MediatR;
using Moq;
using Xunit;

namespace Library.Application.Tests.Behaviors
{
    public class ValidationBehaviorTests
    {
        private readonly Mock<IValidator<TestRequest>> _validatorMock;
        private readonly Mock<RequestHandlerDelegate<TestResponse>> _nextMock;
        private readonly ValidationBehavior<TestRequest, TestResponse> _behavior;

        public ValidationBehaviorTests()
        {
            _validatorMock = new Mock<IValidator<TestRequest>>();
            _nextMock = new Mock<RequestHandlerDelegate<TestResponse>>();
            _behavior = new ValidationBehavior<TestRequest, TestResponse>(new[] { _validatorMock.Object });
        }

        [Fact]
        public async Task Handle_ShouldCallNext_WhenValidationSucceeds()
        {
            // Arrange
            var request = new TestRequest();
            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<TestRequest>>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            // Act
            var response = await _behavior.Handle(request, _nextMock.Object, CancellationToken.None);

            // Assert
            _nextMock.Verify(n => n(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenValidationFails()
        {
            // Arrange
            var request = new TestRequest();
            var failures = new List<FluentValidation.Results.ValidationFailure>
            {
                new FluentValidation.Results.ValidationFailure("Property", "Error message")
            };
            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<TestRequest>>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new FluentValidation.Results.ValidationResult(failures));

            // Act
            Func<Task> act = async () => await _behavior.Handle(request, _nextMock.Object, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                     .WithMessage("Validation failed: \r\n -- Property: Error message Severity: Error");
        }

        [Fact]
        public async Task Handle_ShouldNotCallNext_WhenValidationFails()
        {
            // Arrange
            var request = new TestRequest();
            var failures = new List<FluentValidation.Results.ValidationFailure>
            {
                new FluentValidation.Results.ValidationFailure("Property", "Error message")
            };
            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<TestRequest>>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new FluentValidation.Results.ValidationResult(failures));

            // Act
            Func<Task> act = async () => await _behavior.Handle(request, _nextMock.Object, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
            _nextMock.Verify(n => n(), Times.Never);
        }
    }

    public class TestRequest : IRequest<TestResponse>
    {
    }

    public class TestResponse
    {
    }
}
