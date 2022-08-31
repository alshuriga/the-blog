﻿namespace Blog.Application.Features.Commentary;

public class CommentaryDTO
{
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Text { get; set; } = null!;
    public DateTime DateTime { get; set; }
}


