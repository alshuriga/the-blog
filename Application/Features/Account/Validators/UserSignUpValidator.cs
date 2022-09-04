using Blog.Application.Features.Account.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Features.Account.Validators;

public class UserSignUpValidator : AbstractValidator<UserSignUpDTO>
{
    public UserSignUpValidator()
    {
        RuleFor(u => u.Password).Equal(u => u.RepeatPassword).WithMessage("Passwords don't match.");
    }
}
