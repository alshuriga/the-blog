namespace MiniBlog.Core.Models;

public class EntriesCountData
{
    public int PostsCount { get; set; }
    public Dictionary<long, int> CommentariesCount { get; set; } = new Dictionary<long, int>();
    public int TagsCount { get; set; }
}