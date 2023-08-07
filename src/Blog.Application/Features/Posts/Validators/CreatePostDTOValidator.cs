using Blog.Application.Features.Posts.DTO;
using FluentValidation;
namespace Blog.Application.Features.Posts.Validators
{
    public class CreatePostDTOValidator : AbstractValidator<CreatePostDTO>
    {
        public CreatePostDTOValidator()
        {
            Include(new WritablePostDTOValidator());
        }
    }
}
