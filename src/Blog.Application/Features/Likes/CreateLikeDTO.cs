using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Features.Likes;

public class CreateDeleteLikeDTO
{
    public string Username { get; set; } = null!;
    public long PostId { get; set; }
}
