using FluentValidation;
using Library.Application.Commands.PatchBook;
using Library.Domain.Interfaces;

namespace Library.Application.Validators
{
    public class PatchBookCommandValidator : AbstractValidator<PatchBookCommand>
    {
        public PatchBookCommandValidator(IUnitOfWork unitOfWork)
        {
            Include(new BaseBookCommandValidator(unitOfWork));
        }
    }
}
