using Blog.Application.Features.Posts.DTO.Common;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Features.Posts.Validators
{
    public class IPostDTOValidator : AbstractValidator<IPostDTO>
    {
        public IPostDTOValidator()
        {
            RuleFor(p => p.Header).Length(1, 100);
            RuleFor(p => p.Text).Length(1, 2000);
            RuleFor(p => p.DateTime).GreaterThan(DateTime.Now).WithMessage("Did you invent a time machine?");
            RuleFor(p => p.TagString).Matches(@"^[a-z][a-z0-9_\s,]+[a-z0-9]$").WithMessage("Please use lowercase tags separated by commas, e.g., \"red, green, blue\" for tags list");
        }
    }
}
