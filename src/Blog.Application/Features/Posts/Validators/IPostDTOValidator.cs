using Blog.Application.Features.Posts.DTO.Common;
using FluentValidation;

namespace Blog.Application.Features.Posts.Validators
{
    public class WritablePostDTOValidator : AbstractValidator<WritablePostDTO>
    {
        public WritablePostDTOValidator()
        {
            RuleFor(p => p.Header).Length(1, 100).NotNull().NotEmpty();
            RuleFor(p => p.Text).Length(1, 2000).NotNull().NotEmpty();
            //RuleFor(p => p.TagString).Matches(@"^$|^[a-zA-Z][a-z0-9_\s,]+[a-z0-9]$").WithMessage("Please use words separated by commas, e.g., \"red, green, blue\" for tags");
        }
    }
}
