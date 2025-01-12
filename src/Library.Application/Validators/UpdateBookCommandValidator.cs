using FluentValidation;
using Library.Application.Commands.UpdateBook;

namespace Library.Application.Validators
{
    public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
    {
        public UpdateBookCommandValidator()
        {
            RuleFor(command => command.Author).NotEmpty().MaximumLength(50);
            RuleFor(command => command.Title).NotEmpty().MaximumLength(50); 
            RuleFor(command => command.ISBN).NotEmpty().MaximumLength(20); 
            RuleFor(command => command.Status).NotEmpty().IsInEnum();
        }
    }
}
