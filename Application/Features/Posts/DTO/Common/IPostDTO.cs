namespace Blog.Application.Features.Posts.DTO.Common;

public interface IPostDTO
{
    public string Text { get; set; }
    public string Header { get; set; }
    public string TagString { get; set; }
    public DateTime DateTime { get; set; }
}
