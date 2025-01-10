using Library.Domain.BookStates;
using Library.Domain.Interfaces;

namespace Library.Domain.Entities
{
    public partial class Book
    {
        private IBookState _currentState;
        public Book()
        {
            SetState(new OnTheShelfState()); // Default state
        }

        private void SetState(IBookState bookState)
        {
            _currentState = bookState;
            Status = bookState.Status;
        }

        public virtual void Borrow()
        {
            var nextState = _currentState.Borrow(this);
            SetState(nextState);
        }

        public virtual void Return()
        {
            var nextState = _currentState.Return(this);
            SetState(nextState);
        }

        public virtual void MarkAsDamaged()
        {
            var nextState = _currentState.MarkAsDamaged(this);
            SetState(nextState);
        }

        public virtual void PlaceOnShelf()
        {
            var nextState = _currentState.PlaceOnShelf(this);
            SetState(nextState);
        }
    }
}
