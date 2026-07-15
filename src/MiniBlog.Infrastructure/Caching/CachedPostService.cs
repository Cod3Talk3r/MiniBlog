using Microsoft.Extensions.Caching.Memory;
using MiniBlog.Application.DTOs;
using MiniBlog.Application.Interfaces;

namespace MiniBlog.Infrastructure.Caching
{
    public class CachedPostService : IPostService
    {
        private readonly IPostService _inner;
        private readonly IMemoryCache _cache;
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(1);

        public CachedPostService(IPostService inner, IMemoryCache cache)
        {
            _inner = inner;
            _cache = cache;
        }

        public async Task<(IEnumerable<PostDto> Items, int TotalCount)> GetPagedAsync(
            int page, int pageSize, string? search, CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrWhiteSpace(search) || page != 1)
            {
                return await _inner.GetPagedAsync(page, pageSize, search, cancellationToken);
            }

            var cacheKey = $"posts:page1:size{pageSize}";

            if (_cache.TryGetValue(cacheKey, out (IEnumerable<PostDto> Items, int TotalCount) cached))
            {
                return cached;
            }

            var result = await _inner.GetPagedAsync(page, pageSize, search, cancellationToken);
            _cache.Set(cacheKey, result, CacheDuration);
            return result;
        }

        public Task<PostDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => _inner.GetByIdAsync(id, cancellationToken);

        public Task<PostDto> CreateAsync(CreatePostDto dto, string authorId, CancellationToken cancellationToken = default)
            => _inner.CreateAsync(dto, authorId, cancellationToken);
    }
}