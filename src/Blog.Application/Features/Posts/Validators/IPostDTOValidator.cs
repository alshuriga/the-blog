using Blog.Application.Features.Posts.DTO.Common;
using FluentValidation;

namespace Blog.Application.Features.Posts.Validators
{
    public class IPostDTOValidator : AbstractValidator<IPostDTO>
    {
        public IPostDTOValidator()
        {
            RuleFor(p => p.Header).Length(1, 100).NotNull().NotEmpty();
            RuleFor(p => p.Text).Length(1, 2000).NotNull().NotEmpty();
            RuleFor(p => p.TagString).Matches(@"^$|^[a-z][a-z0-9_\s,]+[a-z0-9]$").WithMessage("Please use lowercase tags separated by commas, e.g., \"red, green, blue\" for tags list");
        }
    }
}
