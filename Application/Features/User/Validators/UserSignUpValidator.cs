using Blog.Application.Features.User.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Features.User.Validators;

public class UserSignUpValidator : AbstractValidator<UserSignUpDTO>
{
    public UserSignUpValidator()
    {
        RuleFor(u => u.Password).Equal(u => u.RepeatPassword).WithMessage("Passwords don't match.");
    }
}
