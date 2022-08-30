namespace MiniBlog.Web.ViewModels;

public class PostDto
{
    public long Id { get; set; }
    public string Header { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public DateTime DateTime { get; set; }
    public bool? IsDraft {  get; set; }
}