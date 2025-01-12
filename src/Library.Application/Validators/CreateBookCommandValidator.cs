using FluentValidation;
using Library.Application.Commands.CreateBook;
using Library.Domain.Interfaces;

namespace Library.Application.Validators
{
    public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateBookCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            RuleFor(command => command.Author).NotEmpty().MaximumLength(50);
            RuleFor(command => command.Title).NotEmpty().MaximumLength(50);
            RuleFor(command => command.ISBN)
                .NotEmpty()
                .MaximumLength(10)
                .MustAsync(BeUnique).WithMessage(command => $"Book with ISBN '{command.ISBN}' already exists");
        }

        private async Task<bool> BeUnique(string ISBN, CancellationToken token)
        {
            return await _unitOfWork.BookRepository.IsISBNUniqueAsync(ISBN);
        }
    }
}
