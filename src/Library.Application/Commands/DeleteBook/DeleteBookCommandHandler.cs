using Library.Domain.Interfaces;
using MediatR;

namespace Library.Application.Commands.DeleteBook
{
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteBookCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _unitOfWork.BookRepository.GetByIdAsync(request.Id);
            _unitOfWork.BeginTransaction();
            await _unitOfWork.BookRepository.DeleteAsync(book);
            _unitOfWork.Commit();

            return Unit.Value;
        }
    }
}
