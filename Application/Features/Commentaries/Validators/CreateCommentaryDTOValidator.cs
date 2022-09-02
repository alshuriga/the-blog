using Blog.Core.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Features.Commentaries.Validators;

public class CreateCommentaryDTOValidator : AbstractValidator<CreateCommentaryDTO>
{
    public CreateCommentaryDTOValidator()
    {
        RuleFor(c => c.Text).Length(1, 300);
        RuleFor(c => c.PostId).GreaterThan(0);
    }
}
