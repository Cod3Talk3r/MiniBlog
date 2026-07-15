using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniBlog.Application.DTOs;
using MiniBlog.Application.Interfaces;

namespace MiniBlog.Web.Pages.Posts
{
    public class IndexModel : PageModel
    {
        private readonly IPostService _postService;

        public IndexModel(IPostService postService)
        {
            _postService = postService;
        }

        public IEnumerable<PostDto> Posts { get; set; } = [];
        public int TotalCount { get; set; }
        public int Page { get; set; } = 1;
        public string? Search { get; set; }
        private const int PageSize = 10;

        public async Task OnGetAsync(int page = 1, string? search = null, CancellationToken cancellationToken = default)
        {
            Page = page;
            Search = search;
            (Posts, TotalCount) = await _postService.GetPagedAsync(page, PageSize, search, cancellationToken);
        }
    }
}