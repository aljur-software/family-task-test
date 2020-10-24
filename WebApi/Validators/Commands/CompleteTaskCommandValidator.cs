using Domain.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Validators.Commands
{
    public class CompleteTaskCommandValidator: AbstractValidator<CompleteTaskCommand>
    {
        public CompleteTaskCommandValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
