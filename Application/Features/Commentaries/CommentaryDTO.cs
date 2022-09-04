﻿namespace Blog.Application.Features.Commentaries;

public class CommentaryDTO
{
    public long Id { get; set; }    
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Text { get; set; } = null!;
    public DateTime DateTime { get; set; }
}


