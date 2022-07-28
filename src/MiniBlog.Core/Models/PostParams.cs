namespace MiniBlog.Core.Models;

public class PaginateParams 
{
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = int.MaxValue;

    public PaginateParams(int skip , int take)
    {
        this.Skip = skip;
        this.Take = take;
    }
    public PaginateParams() {}
}