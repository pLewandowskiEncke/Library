using FluentValidation;
using Library.Application.Commands.PatchBook;

namespace Library.Application.Validators
{
    public class PatchBookCommandValidator : AbstractValidator<PatchBookCommand>
    {
        public PatchBookCommandValidator()
        {
            RuleFor(command => command.Author).MaximumLength(50);
            RuleFor(command => command.Title).MaximumLength(50); 
            RuleFor(command => command.ISBN).MaximumLength(20); 
            RuleFor(command => command.Status).IsInEnum();
        }
    }
}
