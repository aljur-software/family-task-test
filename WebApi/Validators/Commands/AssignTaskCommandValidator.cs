using Domain.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
