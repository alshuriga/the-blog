using Blog.Application.Features.Likes;
using Blog.Application.Features.Posts.DTO.Common;

namespace Blog.Application.Features.Posts.DTO;

public class UpdatePostDTO : WritablePostDTO
{
    public long Id { get; set; }
}
