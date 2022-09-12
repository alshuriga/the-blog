using AutoMapper.Configuration;
using Blog.Application.Features.User.DTO;
using Blog.Application.Interfaces;
using FluentValidation;

namespace Blog.Application.Features.User.Validators;

public class UserSignUpValidator : AbstractValidator<UserSignUpDTO>
{
    public UserSignUpValidator(IUserService userService)
    {
        RuleFor(u => u.Password).NotEmpty().Equal(u => u.RepeatPassword).WithMessage("Passwords don't match.").Length(5, 10);
        RuleFor(u => u.RepeatPassword).NotEmpty().WithMessage("Please repeat a password");
        RuleFor(u => u.Username).NotEmpty();
        RuleFor(u => u.Username).MustAsync(async (u, token) => (await userService.GetUserByNameAsync(u)) == null).WithMessage("User {PropertyValue} already exists.");
        RuleFor(u => u.Email).NotEmpty().EmailAddress().MustAsync(async (e, token) => !(await userService.ListUsersAsync()).Any(u => e?.Trim().ToLower() == u.Email.ToLower())).WithMessage("User with {PropertyValue} e-mail already exists.");
    }
}
