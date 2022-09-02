using Blog.Application.Features.Posts.Requests.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blog.MVC.Controllers
{
    public class PostsController : Controller
    {
        private readonly IMediator _mediator;

        public PostsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("posts/{currentPage?}")]
        public async Task<IActionResult> List(int currentPage = 0)
        {
            var model = await _mediator.Send(new ListPostsPageQuery() {  CurrentPage = 0 });
            return View(model);
        }
    }
}
