using Library.Domain.BookStates;
using Library.Domain.Enums;
using Library.Domain.Interfaces;

namespace Library.Domain.Entities
{
    public partial class Book
    {
        public Book()
        {
            SetState(new OnTheShelfState()); // Default state
        }

        private void SetState(IBookState bookState)
        {
            Status = bookState.Status;
        }

        public virtual void Borrow()
        {
            var nextState = GetStateInstance().Borrow(this);
            SetState(nextState);
        }

        public virtual void Return()
        {
            var nextState = GetStateInstance().Return(this);
            SetState(nextState);
        }

        public virtual void MarkAsDamaged()
        {
            var nextState = GetStateInstance().MarkAsDamaged(this);
            SetState(nextState);
        }

        public virtual void PlaceOnShelf()
        {
            var nextState = GetStateInstance().PlaceOnShelf(this);
            SetState(nextState);
        }

        private IBookState GetStateInstance()
        {
            return Status switch
            {
                BookStatus.OnTheShelf => new OnTheShelfState(),
                BookStatus.Borrowed => new BorrowedState(),
                BookStatus.Returned => new ReturnedState(),
                BookStatus.Damaged => new DamagedState(),
                _ => throw new InvalidOperationException("Invalid state")
            };
        }
    }
}
