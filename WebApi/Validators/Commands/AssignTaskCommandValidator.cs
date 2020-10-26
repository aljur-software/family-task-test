using Domain.Commands;
using FluentValidation;

namespace WebApi.Validators.Commands
{
    public class AssignTaskCommandValidator : AbstractValidator<AssignTaskCommand>
    {
        public AssignTaskCommandValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
            RuleFor(x => x.AssignedMemberId).NotNull().NotEmpty();
        }
    }
}
