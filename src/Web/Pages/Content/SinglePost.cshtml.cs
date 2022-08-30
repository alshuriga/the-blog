using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniBlog.Core.Models;
using MiniBlog.Web.ViewModels;
using MiniBlog.Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using Ardalis.Specification;
using MiniBlog.Core.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using MiniBlog.Web.Extensions;
using MiniBlog.Web.Exceptions;

namespace MiniBlog.Web.Pages;

public class SinglePostModel : PageModel
{
    private readonly IUnitOfWork _unit;
    private readonly UserManager<IdentityUser> _userMgr;
    public PostPartialViewModel PostPartial { get; set; } = null!;
    public PaginationData? CommentsPaginationData { get; set; }
    public IEnumerable<CommentaryDto> Commentaries { get; set; } = Enumerable.Empty<CommentaryDto>();
    public SinglePostModel(IUnitOfWork unit, UserManager<IdentityUser> userMgr)
    {
        _unit = unit;
        _userMgr = userMgr;
    }

    public async Task<IActionResult> OnGet(long postId, int currentPage = 1)
    {
        int commentariesCount = await _unit.commentReadRepo.CountAsync(new CommentsByPostIdSpecification(postId));
        var commentariesPageSpec = new CommentsByPostIdSpecification(postId, currentPage);
        PaginationData? paginationData = PaginationData.CreatePaginationDataOrNull(currentPage, PaginationConstants.COMMENTS_PER_PAGE, commentariesCount);
        Post? post = await _unit.postReadRepo.RetrieveByIdAsync(postId);
        if (post == null) return NotFound();
        if (post.IsDraft) this.ValidateAdminAuth();
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
            TagNames = (await _unit.tagsReadRepo.ListAsync(new TagsByPostIdSpecification(post.Id))).Select(t => t.Name),
            CommentariesCount = commentariesCount,
            IsDraft = post.IsDraft,
        };

        Commentaries = (await _unit.commentReadRepo.ListAsync(commentariesPageSpec)).Select(c => new CommentaryDto
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


    public async Task<IActionResult> OnPostAddComment(CommentaryDto commentary, long postId)
    {
        if (!User.Identity?.IsAuthenticated ?? true) throw new NotLoggedInException();

        if (ModelState.IsValid)
        {
            var user = await _userMgr.FindByNameAsync(User.Identity?.Name);
            if (user != null)
            {
                Commentary comment = new Commentary() { Username = user.UserName, Email = user.Email, Text = commentary.Text, PostId = postId, DateTime = DateTime.Now };
                await _unit.commentRepo.AddAsync(comment);
            }
        }
        return RedirectToPage("SinglePost", routeValues: new { postId = postId });
    }


    public async Task<IActionResult> OnPostDeleteComment(long commentaryId, long postId)
    {
        this.ValidateAdminAuth();
        await _unit.commentRepo.DeleteAsync(new Commentary { Id = commentaryId });
        return RedirectToPage(new {postId = postId});
    }

    public async Task<IActionResult> OnPostDeletePost(long postId)
    {
        this.ValidateAdminAuth();
        await _unit.postRepo.DeleteAsync(new Post { Id = postId });
        return RedirectToPage("List");
    }
}


