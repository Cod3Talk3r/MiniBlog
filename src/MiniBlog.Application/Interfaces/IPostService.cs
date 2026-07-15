using MiniBlog.Application.DTOs;

namespace MiniBlog.Application.Interfaces
{
    public interface IPostService
    {
        Task<(IEnumerable<PostDto> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, string? search, CancellationToken cancellationToken = default);

        Task<PostDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<PostDto> CreateAsync(CreatePostDto dto, string authorId, CancellationToken cancellationToken = default);
    }
}