using Blog.Application.Features.User.DTO;
using FluentValidation;

namespace Blog.Application.Features.User.Validators;

public class UserSignUpValidator : AbstractValidator<UserSignUpDTO>
{
    public UserSignUpValidator()
    {
        RuleFor(u => u.Password).NotNull().Equal(u => u.RepeatPassword).WithMessage("Passwords don't match.");
        RuleFor(u => u.Username).NotEmpty().NotNull();
    }
}
