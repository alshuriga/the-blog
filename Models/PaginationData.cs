namespace MiniBlog.Models;

public class PaginationData
{
    public int CurrentPage { get; set; }
    public int PageNumber { get; set; }
    public string? UrlAddress { get; set; }
}