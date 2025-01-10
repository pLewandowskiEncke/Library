using FluentAssertions;
using Library.Domain.BookStates;
using Library.Domain.Entities;
using Library.Domain.Enums;
using Library.Domain.Interfaces;
using Library.Shared.Exceptions;
using Moq;
using Xunit;

namespace Library.Domain.Tests.Entities
{
    public class BookTests
    {
        private readonly Mock<IBookState> _initialStateMock;
        private readonly Book _book;

        public BookTests()
        {
            _book = new Book();
        }

        #region OnTheShelf State
        [Fact]
        public void Borrow_WhenBookOnTheShelf_ShouldTransitionToNextState()
        {
            // Arrange
            _book.Status.Should().Be(BookStatus.OnTheShelf);

            // Act
            _book.Borrow();

            // Assert
            _book.Status.Should().Be(BookStatus.Borrowed);
        }

        [Fact]
        public void Damage_WhenBookOnTheShelf_ShouldTransitionToNextState()
        {
            // Arrange
            _book.Status.Should().Be(BookStatus.OnTheShelf);

            // Act
            _book.Damage();

            // Assert
            _book.Status.Should().Be(BookStatus.Damaged);
        }

        [Fact]
        public void Return_WhenBookOnTheShelf_ShouldThrowInvalidBookStateException()
        {
            // Arrange
            _book.Status.Should().Be(BookStatus.OnTheShelf);

            // Act
            Action act = () => _book.Return();

            // Assert
            act.Should().Throw<InvalidBookStateException>().WithMessage("Cannot return a book that is on the shelf.");
        }

        [Fact]
        public void PlaceOnShelf_WhenBookOnTheShelf_ShouldThrowInvalidBookStateException()
        {
            // Arrange
            _book.Status.Should().Be(BookStatus.OnTheShelf);

            // Act
            Action act = () => _book.PlaceOnShelf();

            // Assert
            act.Should().Throw<InvalidBookStateException>().WithMessage("The book is already on the shelf.");
        }
        #endregion

        #region Borrowed State
        [Fact]
        public void Return_WhenBookBorrowed_ShouldTransitionToNextState()
        {
            // Arrange
            _book.Borrow();
            _book.Status.Should().Be(BookStatus.Borrowed);

            // Act
            _book.Return();

            // Assert
            _book.Status.Should().Be(BookStatus.Returned);
        }

        [Fact]
        public void Borrow_WhenBookBorrowed_ShouldThrowInvalidBookStateException()
        {
            // Arrange
            _book.Borrow();
            _book.Status.Should().Be(BookStatus.Borrowed);

            // Act
            Action act = () => _book.Borrow();

            // Assert
            act.Should().Throw<InvalidBookStateException>().WithMessage("The book is already borrowed.");
        }

        [Fact]
        public void Damage_WhenBookBorrowed_ShouldThrowInvalidBookStateException()
        {
            // Arrange
            _book.Borrow();
            _book.Status.Should().Be(BookStatus.Borrowed);

            // Act
            Action act = () => _book.Damage();

            // Assert
            act.Should().Throw<InvalidBookStateException>().WithMessage("Cannot damage a borrowed book.");
        }

        [Fact]
        public void PlaceOnShelf_WhenBookBorrowed_ShouldThrowInvalidBookStateException()
        {
            // Arrange
            _book.Borrow();
            _book.Status.Should().Be(BookStatus.Borrowed);

            // Act
            Action act = () => _book.PlaceOnShelf();

            // Assert
            act.Should().Throw<InvalidBookStateException>().WithMessage("Cannot place a borrowed book on the shelf.");
        }
        #endregion

        #region Damaged State
        [Fact]
        public void Return_WhenBookDamaged_ShouldThrowInvalidBookStateException()
        {
            // Arrange
            _book.Damage();
            _book.Status.Should().Be(BookStatus.Damaged);

            // Act
            Action act = () => _book.Return();

            // Assert
            act.Should().Throw<InvalidBookStateException>().WithMessage("Cannot return a damaged book.");
        }

        [Fact]
        public void Borrow_WhenBookDamaged_ShouldThrowInvalidBookStateException()
        {
            // Arrange
            _book.Damage();
            _book.Status.Should().Be(BookStatus.Damaged);

            // Act
            Action act = () => _book.Borrow();

            // Assert
            act.Should().Throw<InvalidBookStateException>().WithMessage("Cannot borrow a damaged book.");
        }

        [Fact]
        public void Damage_WhenBookDamaged_ShouldThrowInvalidBookStateException()
        {
            // Arrange
            _book.Damage();
            _book.Status.Should().Be(BookStatus.Damaged);

            // Act
            Action act = () => _book.Damage();

            // Assert
            act.Should().Throw<InvalidBookStateException>().WithMessage("The book is already damaged.");
        }

        [Fact]
        public void PlaceOnShelf_WhenBookDamaged_ShouldThrowInvalidBookStateException()
        {
            // Arrange
            _book.Damage();
            _book.Status.Should().Be(BookStatus.Damaged);

            // Act
            _book.PlaceOnShelf();

            // Assert
            _book.Status.Should().Be(BookStatus.OnTheShelf);
        }
        #endregion

        #region Returned State
        [Fact]
        public void Return_WhenBookReturned_ShouldThrowInvalidBookStateException()
        {
            // Arrange
            _book.Borrow();
            _book.Return();
            _book.Status.Should().Be(BookStatus.Returned);

            // Act
            Action act = () => _book.Return();

            // Assert
            act.Should().Throw<InvalidBookStateException>().WithMessage("The book is already returned.");
        }

        [Fact]
        public void Borrow_WhenBookReturned_ShouldThrowInvalidBookStateException()
        {
            // Arrange
            _book.Borrow();
            _book.Return();
            _book.Status.Should().Be(BookStatus.Returned);

            // Act
            Action act = () => _book.Borrow();

            // Assert
            act.Should().Throw<InvalidBookStateException>().WithMessage("Cannot borrow a returned book.");
        }

        [Fact]
        public void Damage_WhenBookReturned_ShouldThrowInvalidBookStateException()
        {
            // Arrange
            _book.Borrow();
            _book.Return();
            _book.Status.Should().Be(BookStatus.Returned);

            // Act
            _book.Damage();

            // Assert
            _book.Status.Should().Be(BookStatus.Damaged);
        }

        [Fact]
        public void PlaceOnShelf_WhenBookReturned_ShouldThrowInvalidBookStateException()
        {
            // Arrange
            _book.Borrow();
            _book.Return();
            _book.Status.Should().Be(BookStatus.Returned);

            // Act
            _book.PlaceOnShelf();

            // Assert
            _book.Status.Should().Be(BookStatus.OnTheShelf);
        }
        #endregion
    }
}
