using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniBlog.Web.ViewModels;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MiniBlog.Core.Specifications;
using Ardalis.Specification;
using Microsoft.AspNetCore.Authorization;
using MiniBlog.Web.Extensions;


namespace MiniBlog.Web.Pages;

[Authorize(Roles = RolesConstants.ADMIN_ROLE)]
public class EditorModel : PageModel
{
    private readonly IUnitOfWork _unit;

    [BindProperty]
    public PostDto Post { get; set; } = new();

    [BindProperty]
    [RegularExpression(@"^[a-z][a-z0-9_\s,]+[a-z0-9]$", ErrorMessage = "Please use lowercase tags separated by commas, e.g., \"red, green, blue\"")]
    public string? TagString { get; set; } = String.Empty;

    public EditorModel(IUnitOfWork unit)
    {
        _unit = unit;
    }

    public async Task<IActionResult> OnGet(long postId = 0)
    {
        if (postId != 0)
        {
            ViewData["title"] = "Edit Post";
            Post? post = await _unit.postReadRepo.RetrieveByIdAsync(postId, true);
            if (post is null) return NotFound();
            string tagString = String.Join(",", post.Tags.Select(t => t.Name).AsEnumerable());

            Post = new PostDto
            {
                Id = post.Id,
                Header = post.Header,
                Text = post.Text,
                DateTime = DateTime.Now,
                IsDraft = post.IsDraft
            };
            TagString = tagString;
            return Page();
        }
        ViewData["title"] = "New Post";
        Post = new PostDto();
        return Page();
    }

    public async Task<IActionResult> OnPost(PostDto post, string tagString, bool asDraft = true)
    {
        this.ValidateAdminAuth();

        string[] tagNames = tagString?.Split(",", StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
        if (tagNames.Length > 5) ModelState.AddModelError(nameof(tagString), "Maximum number of tags is 5");
        if (ModelState.IsValid)
        {
            var tags = (await Task.WhenAll(tagNames.Select(async t =>
            {
                Tag tag = (await _unit.tagsReadRepo.ListAsync(new TagsByNameSpecification(t))).FirstOrDefault() ?? new Tag { Name = t };
                return tag;
            }))).ToList();

            Post newPost = await _unit.postReadRepo.RetrieveByIdAsync(post.Id, true) ?? new();
            newPost.Header = post.Header;
            newPost.Text = post.Text;
            newPost.DateTime = post.DateTime;
            newPost.Tags = tags;
            newPost.IsDraft = asDraft;

            if (newPost.Id != default(long))
            {
                await _unit.postRepo.UpdateAsync(newPost);
            }
            else
            {
                await _unit.postRepo.AddAsync(newPost);
            }

            return RedirectToPage("SinglePost", new { postId = newPost.Id });
        }
        else
        {
            return Page();
        }
    }
}