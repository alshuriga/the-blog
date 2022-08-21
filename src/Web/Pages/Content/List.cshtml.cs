using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniBlog.Core.Models;
using MiniBlog.Web.ViewModels;
using MiniBlog.Core.Specifications;
using MiniBlog.Core.Constants;
using Ardalis.Specification;
using Microsoft.AspNetCore.Mvc;
using MiniBlog.Web.Extensions;

namespace MiniBlog.Web.Pages;

public class ListModel : PageModel
{
    private readonly IUnitOfWork _unit;
    public IEnumerable<PostPartialViewModel> Posts { get; set; } = Enumerable.Empty<PostPartialViewModel>();
    public PaginationData? PaginationData { get; set; }
    public int? PostsCount { get; set; }
    public string? TagName { get; set; }
    public string? UrlAddress { get; set; }
    public bool IsDrafts { get; set; }

    public ListModel(IUnitOfWork unit)
    {
        _unit = unit;
    }

    public async Task<IActionResult> OnGet(int currentPage = 1, string? tagName = null, bool draft = false)
    {
        if(draft) this.ValidateAdminAuth();

        IsDrafts = draft;

        Tag? tag = tagName == null ? null : new() { Name = tagName };

        ISpecification<Post> postsSpecification = new PostsByPageSpecification(currentPage, tag, eager: true, isDraft: draft);

        int postsCount = await _unit.postReadRepo.CountAsync(new PostsByTagSpecification(tag, isDraft: draft));

        PaginationData? paginationData = PaginationData.CreatePaginationDataOrNull(currentPage, PaginationConstants.POSTS_PER_PAGE, postsCount);

        var posts = ((await _unit.postReadRepo.ListAsync(postsSpecification)).Select(p => new PostPartialViewModel
        {
            Post = new PostDto
            {
                Id = p.Id,
                Header = p.Header,
                DateTime = p.DateTime,
                Text = p.Text
            },
            TagNames = p.Tags.Select(t => t.Name),
            CommentsButton = true,
            CommentariesCount = _unit.commentReadRepo.CountAsync(new CommentsByPostIdSpecification(p.Id)).Result,
            IsDraft = draft
        }));

        Posts = posts;
        PaginationData = paginationData;
        TagName = tagName;
        PostsCount = postsCount;

        return Page();
    }

}