using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniBlog.Application.DTOs;
using MiniBlog.Application.Interfaces;
using System.Security.Claims;

namespace MiniBlog.Web.Pages.Posts
{
    public class DetailsModel : PageModel
    {
        private readonly IPostService _postService;
        private readonly ICommentService _commentService;

        public DetailsModel(IPostService postService, ICommentService commentService)
        {
            _postService = postService;
            _commentService = commentService;
        }

        public PostDto? Post { get; set; }
        public IEnumerable<CommentDto> Comments { get; set; } = [];

        [BindProperty]
        public string CommentText { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
        {
            Post = await _postService.GetByIdAsync(id, cancellationToken);
            if (Post is null) return NotFound();

            Comments = await _commentService.GetByPostIdAsync(id, cancellationToken);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken)
        {
            if (User.Identity?.IsAuthenticated != true)
                return RedirectToPage("/Account/Login");

            var userId = User.FindFirstValue("sub") ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null) return RedirectToPage("/Account/Login");

            if (!string.IsNullOrWhiteSpace(CommentText))
            {
                await _commentService.AddAsync(id, new CreateCommentDto(CommentText), userId, cancellationToken);
            }

            return RedirectToPage(new { id });
        }
    }
}