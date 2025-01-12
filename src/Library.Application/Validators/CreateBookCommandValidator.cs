using FluentValidation;
using Library.Application.Commands.CreateBook;
using Library.Domain.Interfaces;

namespace Library.Application.Validators
{
    public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
    {
        public CreateBookCommandValidator(IUnitOfWork unitOfWork)
        {
            Include(new BaseBookCommandValidator(unitOfWork));
            // Adding rulse on top of the base rules
            RuleFor(command => command.Author).NotEmpty();
            RuleFor(command => command.Title).NotEmpty();
            RuleFor(command => command.ISBN).NotEmpty();
        }
    }
}
