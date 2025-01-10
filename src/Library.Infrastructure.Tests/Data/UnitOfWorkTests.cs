using FluentAssertions;
using Library.Domain.Interfaces;
using Library.Infrastructure.Data;
using Moq;
using NHibernate;
using Xunit;

namespace Library.Infrastructure.Tests.Data
{
    public class UnitOfWorkTests : IDisposable
    {
        private readonly Mock<ISession> _sessionMock;
        private readonly Mock<ITransaction> _transactionMock;
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly UnitOfWork _unitOfWork;

        public UnitOfWorkTests()
        {
            _sessionMock = new Mock<ISession>();
            _transactionMock = new Mock<ITransaction>();
            _bookRepositoryMock = new Mock<IBookRepository>();
            _unitOfWork = new UnitOfWork(_sessionMock.Object, _bookRepositoryMock.Object);
        }

        [Fact]
        public void BookRepository_ShouldReturnInjectedRepository()
        {
            // Act
            var result = _unitOfWork.BookRepository;

            // Assert
            result.Should().Be(_bookRepositoryMock.Object);
        }

        [Fact]
        public void BeginTransaction_ShouldBeginTransaction()
        {
            // Arrange
            _sessionMock.Setup(s => s.BeginTransaction()).Returns(_transactionMock.Object);

            // Act
            _unitOfWork.BeginTransaction();

            // Assert
            _sessionMock.Verify(s => s.BeginTransaction(), Times.Once);
        }

        [Fact]
        public void Commit_ShouldCommitTransaction_WhenTransactionIsActive()
        {
            // Arrange
            _sessionMock.Setup(s => s.BeginTransaction()).Returns(_transactionMock.Object);
            _unitOfWork.BeginTransaction();
            _transactionMock.Setup(t => t.IsActive).Returns(true);

            // Act
            _unitOfWork.Commit();

            // Assert
            _transactionMock.Verify(t => t.Commit(), Times.Once);
            _transactionMock.Verify(t => t.Dispose(), Times.Once);
        }

        [Fact]
        public void Commit_ShouldRollbackTransaction_WhenCommitFails()
        {
            // Arrange
            _sessionMock.Setup(s => s.BeginTransaction()).Returns(_transactionMock.Object);
            _unitOfWork.BeginTransaction();
            _transactionMock.Setup(t => t.IsActive).Returns(true);
            _transactionMock.Setup(t => t.Commit()).Throws<Exception>();

            // Act
            Action act = () => _unitOfWork.Commit();

            // Assert
            act.Should().Throw<Exception>();
            _transactionMock.Verify(t => t.Rollback(), Times.Once);
            _transactionMock.Verify(t => t.Dispose(), Times.Once);
        }

        [Fact]
        public void Rollback_ShouldRollbackTransaction_WhenTransactionIsActive()
        {
            // Arrange
            _sessionMock.Setup(s => s.BeginTransaction()).Returns(_transactionMock.Object);
            _unitOfWork.BeginTransaction();
            _transactionMock.Setup(t => t.IsActive).Returns(true);

            // Act
            _unitOfWork.Rollback();

            // Assert
            _transactionMock.Verify(t => t.Rollback(), Times.Once);
            _transactionMock.Verify(t => t.Dispose(), Times.Once);
        }

        [Fact]
        public void Dispose_ShouldDisposeSession()
        {
            // Act
            _unitOfWork.Dispose();

            // Assert
            _sessionMock.Verify(s => s.Dispose(), Times.Once);
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
