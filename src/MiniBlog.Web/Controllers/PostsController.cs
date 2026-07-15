using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniBlog.Application.DTOs;
using MiniBlog.Application.Interfaces;
using System.Security.Claims;

namespace MiniBlog.Web.Controllers
{

    [ApiController]
    [Route("api/posts")]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            CancellationToken cancellationToken = default)
        {
            var (items, totalCount) = await _postService.GetPagedAsync(page, pageSize, search, cancellationToken);
            return Ok(new { items, totalCount, page, pageSize });
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<PostDto>> GetById(int id, CancellationToken cancellationToken)
        {
            var post = await _postService.GetByIdAsync(id, cancellationToken);
            return post is null ? NotFound() : Ok(post);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<PostDto>> Create(CreatePostDto dto, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue("sub") ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null) return Unauthorized();

            var created = await _postService.CreateAsync(dto, userId, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
    }
}