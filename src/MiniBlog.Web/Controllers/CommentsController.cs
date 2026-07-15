using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniBlog.Application.DTOs;
using MiniBlog.Application.Interfaces;
using System.Security.Claims;

namespace MiniBlog.Web.Controllers
{
    [ApiController]
    [Route("api/posts/{postId:int}/comments")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetByPost(int postId, CancellationToken cancellationToken)
        {
            var comments = await _commentService.GetByPostIdAsync(postId, cancellationToken);
            return Ok(comments);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CommentDto>> Add(int postId, CreateCommentDto dto, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue("sub") ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null) return Unauthorized();

            var comment = await _commentService.AddAsync(postId, dto, userId, cancellationToken);
            return Ok(comment);
        }
    }
}