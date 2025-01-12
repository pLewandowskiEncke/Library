using FluentValidation;
using Library.Application.Commands.UpdateBook;
using Library.Domain.Interfaces;

namespace Library.Application.Validators
{
    public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
    {
        public UpdateBookCommandValidator(IUnitOfWork unitOfWork)
        {
            Include(new BaseBookCommandValidator(unitOfWork));
            // Adding rulse on top of the base rules
            RuleFor(command => command.Author).NotEmpty();
            RuleFor(command => command.Title).NotEmpty();
            RuleFor(command => command.ISBN).NotEmpty();

        }
    }
}
