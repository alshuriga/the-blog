using Blog.Application.Features.Posts.DTO;
using FluentValidation;

namespace Blog.Application.Features.Posts.Validators
{
    public class UpdatePostDTOValidator : AbstractValidator<CreatePostDTO>
    {
        public UpdatePostDTOValidator()
        {
            Include(new IPostDTOValidator());
        }
    }
}
