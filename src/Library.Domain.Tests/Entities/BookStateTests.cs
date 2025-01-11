using FluentAssertions;
using Library.Domain.Entities;
using Library.Domain.Enums;
using Library.Shared.Exceptions;
using Xunit;

namespace Library.Domain.Tests.Entities
{
    public class BookStateTests
    {
        private Book _book;

        public BookStateTests()
        {
            _book = new Book();
        }

        #region PlaceOnShelf
        [Fact]
        public void PlaceOnShelf_WhenBookDamaged_ShouldThrowInvalidBookStateException()
        {
            // Arrange
            _book = new CustomBook(BookStatus.Damaged);

            // Act
            _book.PlaceOnShelf();

            // Assert
            _book.Status.Should().Be(BookStatus.OnTheShelf);
        }

        [Fact]
        public void PlaceOnShelf_WhenBookReturned_ShouldThrowInvalidBookStateException()
        {
            // Arrange
            _book = new CustomBook(BookStatus.Returned);

            // Act
            _book.PlaceOnShelf();

            // Assert
            _book.Status.Should().Be(BookStatus.OnTheShelf);
        }

        [Fact]
        public void PlaceOnShelf_WhenBookOnTheShelf_ShouldThrowInvalidBookStateException()
        {
            // Arrange
            _book = new CustomBook(BookStatus.OnTheShelf);

            // Act
            Action act = () => _book.PlaceOnShelf();

            // Assert
            act.Should().Throw<InvalidBookStateException>().WithMessage("The book is already on the shelf.");
        }

        [Fact]
        public void PlaceOnShelf_WhenBookBorrowed_ShouldThrowInvalidBookStateException()
        {
            // Arrange
            _book = new CustomBook(BookStatus.Borrowed);

            // Act
            Action act = () => _book.PlaceOnShelf();

            // Assert
            act.Should().Throw<InvalidBookStateException>().WithMessage("Cannot place a borrowed book on the shelf.");
        }
        #endregion

        #region Borrow
        [Fact]
        public void Borrow_WhenBookOnTheShelf_ShouldTransitionToNextState()
        {
            // Arrange
            _book = new CustomBook(BookStatus.OnTheShelf);

            // Act
            _book.Borrow();

            // Assert
            _book.Status.Should().Be(BookStatus.Borrowed);
        }

        [Fact]
        public void Borrow_WhenBookBorrowed_ShouldThrowInvalidBookStateException()
        {
            // Arrange
            _book = new CustomBook(BookStatus.Borrowed);

            // Act
            Action act = () => _book.Borrow();

            // Assert
            act.Should().Throw<InvalidBookStateException>().WithMessage("The book is already borrowed.");
        }

        [Fact]
        public void Borrow_WhenBookDamaged_ShouldThrowInvalidBookStateException()
        {
            // Arrange
            _book = new CustomBook(BookStatus.Damaged);

            // Act
            Action act = () => _book.Borrow();

            // Assert
            act.Should().Throw<InvalidBookStateException>().WithMessage("Cannot borrow a damaged book.");
        }

        [Fact]
        public void Borrow_WhenBookReturned_ShouldThrowInvalidBookStateException()
        {
            // Arrange
            _book = new CustomBook(BookStatus.Returned);

            // Act
            Action act = () => _book.Borrow();

            // Assert
            act.Should().Throw<InvalidBookStateException>().WithMessage("Cannot borrow a returned book.");
        }
        #endregion

        #region MarkAsDamaged
        [Fact]
        public void MarkAsDamaged_WhenBookOnTheShelf_ShouldTransitionToNextState()
        {
            // Arrange
            _book = new CustomBook(BookStatus.OnTheShelf);

            // Act
            _book.MarkAsDamaged();

            // Assert
            _book.Status.Should().Be(BookStatus.Damaged);
        }

        [Fact]
        public void MarkAsDamaged_WhenBookReturned_ShouldTransitionToNextState()
        {
            // Arrange            
            _book = new CustomBook(BookStatus.Returned);

            // Act
            _book.MarkAsDamaged();

            // Assert
            _book.Status.Should().Be(BookStatus.Damaged);
        }

        [Fact]
        public void MarkAsDamaged_WhenBookBorrowed_ShouldThrowInvalidBookStateException()
        {
            // Arrange
            _book = new CustomBook(BookStatus.Borrowed);

            // Act
            Action act = () => _book.MarkAsDamaged();

            // Assert
            act.Should().Throw<InvalidBookStateException>().WithMessage("Cannot mark as damaged a borrowed book.");
        }

        [Fact]
        public void MarkAsDamaged_WhenBookDamaged_ShouldThrowInvalidBookStateException()
        {
            // Arrange
            _book = new CustomBook(BookStatus.Damaged);

            // Act
            Action act = () => _book.MarkAsDamaged();

            // Assert
            act.Should().Throw<InvalidBookStateException>().WithMessage("The book is already marked as damaged.");
        }
        #endregion

        #region Return

        [Fact]
        public void Return_WhenBookBorrowed_ShouldTransitionToNextState()
        {
            // Arrange
            _book = new CustomBook(BookStatus.Borrowed);

            // Act
            _book.Return();

            // Assert
            _book.Status.Should().Be(BookStatus.Returned);
        }

        [Fact]
        public void Return_WhenBookOnTheShelf_ShouldThrowInvalidBookStateException()
        {
            // Arrange
            _book = new CustomBook(BookStatus.OnTheShelf);

            // Act
            Action act = () => _book.Return();

            // Assert
            act.Should().Throw<InvalidBookStateException>().WithMessage("Cannot return a book that is on the shelf.");
        }
         
        [Fact]
        public void Return_WhenBookDamaged_ShouldThrowInvalidBookStateException()
        {
            // Arrange
            _book = new CustomBook(BookStatus.Damaged);

            // Act
            Action act = () => _book.Return();

            // Assert
            act.Should().Throw<InvalidBookStateException>().WithMessage("Cannot return a damaged book.");
        }
        
        [Fact]
        public void Return_WhenBookReturned_ShouldThrowInvalidBookStateException()
        {
            // Arrange            
            _book = new CustomBook(BookStatus.Returned);

            // Act
            Action act = () => _book.Return();

            // Assert
            act.Should().Throw<InvalidBookStateException>().WithMessage("The book is already returned.");
        }
        #endregion

        #region TryChangeStatus
        [Fact]
        public void TryChangeStatus_ToOnTheShelf_ChangesStatus()
        {
            // Arrange
            var book = new CustomBook(BookStatus.Returned);

            // Act
            book.TryChangeStatus(BookStatus.OnTheShelf);

            // Assert
            book.Status.Should().Be(BookStatus.OnTheShelf);
        }

        [Fact]
        public void TryChangeStatus_ToBorrowed_ChangesStatus()
        {
            // Arrange
            var book = new CustomBook(BookStatus.OnTheShelf);

            // Act
            book.TryChangeStatus(BookStatus.Borrowed);

            // Assert
            book.Status.Should().Be(BookStatus.Borrowed);
        }

        [Fact]
        public void TryChangeStatus_ToReturned_ChangesStatus()
        {
            // Arrange
            var book = new CustomBook(BookStatus.Borrowed);

            // Act
            book.TryChangeStatus(BookStatus.Returned);

            // Assert
            book.Status.Should().Be(BookStatus.Returned);
        }

        [Fact]
        public void TryChangeStatus_ToDamaged_ChangesStatus()
        {
            // Arrange
            var book = new CustomBook(BookStatus.OnTheShelf);

            // Act
            book.TryChangeStatus(BookStatus.Damaged);

            // Assert
            book.Status.Should().Be(BookStatus.Damaged);
        }

        [Fact]
        public void TryChangeStatus_InvalidStatus_ThrowsInvalidBookStateException()
        {
            // Arrange
            var book = new CustomBook(BookStatus.OnTheShelf);

            // Act
            Action act = () => book.TryChangeStatus((BookStatus)999);

            // Assert
            act.Should().Throw<InvalidBookStateException>()
               .WithMessage("Invalid state");
        }
        #endregion
    }

    internal class CustomBook : Book
    {
        public CustomBook(BookStatus status)
        {
            Status = status;
        }
    }
}
