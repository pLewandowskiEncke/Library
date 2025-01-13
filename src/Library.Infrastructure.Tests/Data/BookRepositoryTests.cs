
using FluentAssertions;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Library.Domain.Entities;
using Library.Infrastructure.Data;
using Library.Infrastructure.Data.Mappings;
using Library.Shared.Exceptions;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using Xunit;

namespace Library.Infrastructure.Tests.Data
{
    public class BookRepositoryTests
    {
        private readonly ISession _session;
        private readonly BookRepository _bookRepository;

        public BookRepositoryTests()
        {
            _session = CreateSession();
            _bookRepository = new BookRepository(_session);
        }

        private static ISession CreateSession()
        {
            Configuration configuration = null;

            // Manually configuring Map. NHibernate supports conventions and advanced features as well
            var factory = Fluently.Configure()
                           .Mappings(x => x.FluentMappings.Add(typeof(BookMap)))
                           .Database(SQLiteConfiguration.Standard.InMemory().ShowSql())
                           .ExposeConfiguration(cfg => configuration = cfg)
                           .BuildSessionFactory();

            var session = factory.OpenSession();

            new SchemaExport(configuration).Execute(true, true, false, session.Connection, null);

            return session;
        }

        [Fact]
        public async Task AddAsync_ShouldAddBook()
        {
            // Arrange
            var book = new Book { Title = "Test Book", Author = "Test Author", ISBN = "1234567890" };

            // Act
            await _bookRepository.AddAsync(book);
            _session.Flush();
            _session.Clear();

            // Assert
            var retrievedBook = await _session.GetAsync<Book>(book.Id);
            retrievedBook.Should().NotBeNull();
            retrievedBook.Title.Should().Be("Test Book");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnBook_WhenBookExists()
        {
            // Arrange
            var book = new Book { Title = "Test Book", Author = "Test Author", ISBN = "1234567890" };
            await _session.SaveAsync(book);
            _session.Flush();
            _session.Clear();

            // Act
            var result = await _bookRepository.GetByIdAsync(book.Id);

            // Assert
            result.Should().NotBeNull();
            result.Title.Should().Be("Test Book");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowNotFoundException_WhenBookNotFound()
        {
            // Arrange
            var book = new Book { Title = "Test Book", Author = "Test Author", ISBN = "1234567890" };
            await _session.SaveAsync(book);
            _session.Flush();
            _session.Clear();

            // Act
            Func<Task> act = async () => await _bookRepository.GetByIdAsync(99);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateBook()
        {
            // Arrange
            var book = new Book { Title = "Test Book", Author = "Test Author", ISBN = "1234567890" };
            await _session.SaveAsync(book);
            _session.Flush();
            _session.Clear();

            // Act
            book.Title = "Updated Test Book";
            await _bookRepository.UpdateAsync(book);
            _session.Flush();
            _session.Clear();

            // Assert
            var retrievedBook = await _session.GetAsync<Book>(book.Id);
            retrievedBook.Title.Should().Be("Updated Test Book");
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveBook()
        {
            // Arrange
            var book = new Book { Title = "Test Book", Author = "Test Author", ISBN = "1234567890" };
            await _session.SaveAsync(book);
            _session.Flush();
            _session.Clear();

            // Act
            await _bookRepository.DeleteAsync(book);
            _session.Flush();
            _session.Clear();

            // Assert
            var retrievedBook = await _session.GetAsync<Book>(book.Id);
            retrievedBook.Should().BeNull();

        }

        [Fact]
        public async Task GetBooks_ShouldReturnPagedBooks()
        {
            // Arrange
            for (int i = 1; i <= 9; i++)
            {
                var book = new Book { Title = $"Test Book {i}", Author = "Test Author", ISBN = $"123456789{i}" };
                await _session.SaveAsync(book);
            }
            _session.Flush();
            _session.Clear();

            // Act
            var books = await _bookRepository.GetBooks(1, 5, "Title", true);

            // Assert
            books.Should().HaveCount(5);
            books.Should().Contain(b => b.Title == "Test Book 1");
            books.Should().Contain(b => b.Title == "Test Book 5");
        }

        [Fact]
        public async Task IsISBNUniqueAsync_ShouldReturnTrue_WhenNoBooksWithTheSameISBN()
        {
            // Arrange
            var isbn = "1234567890";

            // Act
            var result = await _bookRepository.IsISBNUniqueAsync(isbn, null);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task IsISBNUniqueAsync_ShouldReturnTrue_WhenBookWithSameIdHasSameISBN()
        {
            // Arrange
            var book = new Book { Title = "Test Book 1", Author = "Test Author", ISBN = "1234567890" };
            await _session.SaveAsync(book);
            _session.Flush();
            _session.Clear();

            // Act
            var result = await _bookRepository.IsISBNUniqueAsync(book.ISBN, book.Id);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task IsISBNUniqueAsync_ShouldReturnFalse_WhenAnotherBookExistsWithSameISBN()
        {
            // Arrange
            var book = new Book { Title = "Test Book", Author = "Test Author", ISBN = "1234567890" };
            await _session.SaveAsync(book);
            _session.Flush();
            _session.Clear();

            // Act
            var result = await _bookRepository.IsISBNUniqueAsync(book.ISBN, 99);

            // Assert
            result.Should().BeFalse();
        }
    }
}
