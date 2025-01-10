
using FluentAssertions;
using Library.Domain.Entities;
using Library.Infrastructure.Data;
using Moq;
using NHibernate;
using NHibernate.Criterion;
using Xunit;

namespace Library.Infrastructure.Tests.Data
{
    public class BookRepositoryTests
    {
        private readonly Mock<ISession> _sessionMock;
        private readonly BookRepository _bookRepository;

        public BookRepositoryTests()
        {
            _sessionMock = new Mock<ISession>();
            _bookRepository = new BookRepository(_sessionMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnBook_WhenBookExists()
        {
            // Arrange
            var bookId = 1;
            var expectedBook = new Book { Id = bookId };
            _sessionMock.Setup(s => s.GetAsync<Book>(bookId, It.IsAny<CancellationToken>())).ReturnsAsync(expectedBook);

            // Act
            var result = await _bookRepository.GetByIdAsync(bookId);

            // Assert
            result.Should().Be(expectedBook);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllBooks()
        {
            // Arrange
            var expectedBooks = new List<Book> { new Book { Id = 1 }, new Book { Id = 2 } };
            _sessionMock.Setup(s => s.QueryOver<Book>().ListAsync<Book>(It.IsAny<CancellationToken>())).ReturnsAsync(expectedBooks);

            // Act
            var result = await _bookRepository.GetAllAsync();

            // Assert
            result.Should().BeEquivalentTo(expectedBooks);
        }

        [Fact]
        public async Task AddAsync_ShouldAddBook()
        {
            // Arrange
            var book = new Book { Id = 1 };
            _sessionMock.Setup(s => s.SaveAsync(book, It.IsAny<CancellationToken>())).Returns(Task.FromResult<object>(null));

            // Act
            await _bookRepository.AddAsync(book);

            // Assert
            _sessionMock.Verify(s => s.SaveAsync(book, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateBook()
        {
            // Arrange
            var book = new Book { Id = 1 };
            _sessionMock.Setup(s => s.UpdateAsync(book, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            // Act
            await _bookRepository.UpdateAsync(book);

            // Assert
            _sessionMock.Verify(s => s.UpdateAsync(book, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteBook()
        {
            // Arrange
            var book = new Book { Id = 1 };
            _sessionMock.Setup(s => s.DeleteAsync(book, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            // Act
            await _bookRepository.DeleteAsync(book);

            // Assert
            _sessionMock.Verify(s => s.DeleteAsync(book, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetBooks_ShouldReturnPagedAndSortedBooks()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;
            var sortBy = "Title";
            var ascending = true;
            var expectedBooks = new List<Book> { new Book { Id = 1 }, new Book { Id = 2 } };

            var criteriaMock = new Mock<ICriteria>();
            criteriaMock.Setup(c => c.AddOrder(It.IsAny<Order>())).Returns(criteriaMock.Object);
            criteriaMock.Setup(c => c.SetFirstResult(It.IsAny<int>())).Returns(criteriaMock.Object);
            criteriaMock.Setup(c => c.SetMaxResults(It.IsAny<int>())).Returns(criteriaMock.Object);
            criteriaMock.Setup(c => c.ListAsync<Book>(It.IsAny<CancellationToken>())).ReturnsAsync(expectedBooks);

            _sessionMock.Setup(s => s.CreateCriteria<Book>()).Returns(criteriaMock.Object);

            // Act
            var result = await _bookRepository.GetBooks(pageNumber, pageSize, sortBy, ascending);

            // Assert
            result.Should().BeEquivalentTo(expectedBooks);
            criteriaMock.Verify(c => c.AddOrder(It.Is<Order>(o => o.ToString() == $"{sortBy} {(ascending ? "asc" : "desc")}")), Times.Once);
            criteriaMock.Verify(c => c.SetFirstResult((pageNumber - 1) * pageSize), Times.Once);
            criteriaMock.Verify(c => c.SetMaxResults(pageSize), Times.Once);
        }
    }
}
