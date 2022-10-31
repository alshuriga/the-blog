using Blog.Application.Features.User.DTO;
using FluentValidation;

namespace Blog.Application.Features.User.Validators;

public class UserSignInValidator : AbstractValidator<UserSignInDTO>
{
    public UserSignInValidator()
    {
        RuleFor(u => u.Username).NotEmpty();
        RuleFor(u => u.Password).NotEmpty();
    }
}
