﻿using Blog.Application.Features.Likes;
using Blog.Application.Features.Tags.DTO;

namespace Blog.Application.Features.Posts.DTO;

public class PostListVM
{
    public long Id { get; set; }
    public string Text { get; set; } = null!;
    public string Header { get; set; } = null!;
    public DateTime DateTime { get; set; }
    public int CommentariesCount { get; set; }
    public bool IsDraft { get; set; }
    public List<TagDTO> Tags { get; set; } = new List<TagDTO>();
    public List<LikeDTO> Likes { get; set; } = new List<LikeDTO>();
}
