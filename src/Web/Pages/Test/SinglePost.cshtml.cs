using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniBlog.Core.Models;
using MiniBlog.Web.ViewModels;
using MiniBlog.Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using Ardalis.Specification;
using MiniBlog.Core.Constants;

namespace MiniBlog.Web.Pages;

public class SinglePostModel : PageModel
{
    private readonly IUnitOfWork _unit;
    public PostPartialViewModel? PostPartial { get; set; }
    public PaginationData? CommentsPaginationData { get; set; }
    public IEnumerable<CommentaryDto> Commentaries { get; set; } = Enumerable.Empty<CommentaryDto>();

    public SinglePostModel(IUnitOfWork unit)
    {
        _unit = unit;
    }

    public async Task<IActionResult> OnGet(long postId, int currentPage = 1)
    {
        int commentsCount = await _unit.commentReadRepo.CountAsync(new CommentsByPostIdSpecification(postId));
        PaginationData? paginationData = PaginationData.CreatePaginationDataOrNull(currentPage, PaginationConstants.COMMENTS_PER_PAGE, commentsCount);
        PaginateParams postParams = new(paginationData?.SkipNumber ?? 0, PaginationConstants.COMMENTS_PER_PAGE);
        Post? post = await _unit.postReadRepo.RetrieveByIdAsync(postId, true);
        if (post == null) return NotFound();
        TempData["postId"] = postId.ToString();

        PostPartial = new()
        {
            Post = new()
            {
                Id = post.Id,
                Header = post.Header,
                Text = post.Text,
                DateTime = post.DateTime
            },
            CommentsButton = false,
            TagNames = post.Tags.Select(t => t.Name)
        };

        Commentaries = post.Commentaries.Select(c => new CommentaryDto
        {
            Id = c.Id,
            Username = c.Username,
            Email = c.Email,
            Text = c.Text,
            DateTime = c.DateTime
        });
        CommentsPaginationData = paginationData;

        return Page();
    }
    
}
