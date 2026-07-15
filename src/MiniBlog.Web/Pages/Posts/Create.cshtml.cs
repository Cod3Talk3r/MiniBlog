using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniBlog.Application.DTOs;
using MiniBlog.Application.Interfaces;
using System.Security.Claims;

namespace MiniBlog.Web.Pages.Posts
{
    public class CreateModel : PageModel
    {
        private readonly IPostService _postService;

        public CreateModel(IPostService postService)
        {
            _postService = postService;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            public string Title { get; set; } = default!;
            public string Content { get; set; } = default!;
        }

        public IActionResult OnGet()
        {
            if (User.Identity?.IsAuthenticated != true)
                return RedirectToPage("/Account/Login");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (User.Identity?.IsAuthenticated != true)
                return RedirectToPage("/Account/Login");

            if (!ModelState.IsValid) return Page();

            var userId = User.FindFirstValue("sub") ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null) return RedirectToPage("/Account/Login");

            var post = await _postService.CreateAsync(new CreatePostDto(Input.Title, Input.Content), userId, cancellationToken);

            return RedirectToPage("/Posts/Details", new { id = post.Id });
        }
    }
}