using Blog.Application.Features.Posts.DTO;
using FluentValidation;

namespace Blog.Application.Features.Posts.Validators
{
    public class UpdatePostDTOValidator : AbstractValidator<UpdatePostDTO>
    {
        public UpdatePostDTOValidator()
        {
            Include(new WritablePostDTOValidator());
        }
    }
}
