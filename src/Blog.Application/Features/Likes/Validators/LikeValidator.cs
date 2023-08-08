using Blog.Application.Interfaces.Common;
using Blog.Core.Entities;
using FluentValidation;

namespace Blog.Application.Features.Likes.Validators
{
    public class LikeValidator : AbstractValidator<CreateDeleteLikeDTO>
    {
        public LikeValidator(IBlogRepository<Post> postRepo)
        {
            RuleFor(l => l).MustAsync(async (e, token) => !(await postRepo.GetByIdAsync(e.PostId))!.Likes.Any(p => p.Username == e.Username));
        }
    }
}
