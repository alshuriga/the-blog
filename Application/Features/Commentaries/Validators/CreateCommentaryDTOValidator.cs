using Blog.Application.Interfaces.Common;
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
    private readonly IBlogRepository<Post> _repo;
    public CreateCommentaryDTOValidator(IBlogRepository<Post> repo)
    {
        _repo = repo;

        RuleFor(c => c.PostId).CustomAsync(async (id, context, cancelation) =>
        {
            if(await _repo.GetByIdAsync(id) == null)
            {
                context.AddFailure($"Post {id} not found");
            }
        });

        RuleFor(c => c.Text).Length(1, 300).NotEmpty();
    }
}
