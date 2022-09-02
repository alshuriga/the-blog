using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Features.Commentaries;

public class CreateCommentaryDTO
{
    public string Text { get; set; } = null!;
    public long PostId { get; set; }
}

