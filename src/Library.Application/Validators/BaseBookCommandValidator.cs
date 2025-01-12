using FluentValidation;
using Library.Application.Commands;
using Library.Domain.Interfaces;

namespace Library.Application.Validators
{
    public class BaseBookCommandValidator : AbstractValidator<BaseBookCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public BaseBookCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            RuleFor(command => command.Author)
                .MinimumLength(1)
                .MaximumLength(50)
                .When(command => command.Author != null);
            RuleFor(command => command.Title)
                .MinimumLength(1)
                .MaximumLength(50)
                .When(command => command.Title != null);
            RuleFor(command => command.ISBN)
                .MinimumLength(1)
                .MaximumLength(50)
                .MustAsync(BeUnique).WithMessage(command => $"Book with ISBN '{command.ISBN}' already exists")
                .When(command => command.ISBN != null);
        }

        private async Task<bool> BeUnique(string ISBN, CancellationToken token)
        {
            return await _unitOfWork.BookRepository.IsISBNUniqueAsync(ISBN);
        }
    }
}
