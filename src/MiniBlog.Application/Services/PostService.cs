using MiniBlog.Application.DTOs;
using MiniBlog.Application.Interfaces;
using MiniBlog.Domain.Entities;
using MiniBlog.Domain.Interfaces;

namespace MiniBlog.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly AutoMapper.IMapper _mapper;

        public PostService(IPostRepository postRepository, AutoMapper.IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<PostDto> Items, int TotalCount)> GetPagedAsync(
            int page, int pageSize, string? search, CancellationToken cancellationToken = default)
        {
            // IQueryable => Didn't execute the query yet, just build it
            IQueryable<Post> query = _postRepository.GetAllQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p => p.Title.Contains(search) || p.Content.Contains(search));
            }

            var totalCount = await Task.Run(() => query.Count(), cancellationToken);

            var posts = query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var dtos = _mapper.Map<IEnumerable<PostDto>>(posts);
            return (dtos, totalCount);
        }

        public async Task<PostDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var post = await _postRepository.GetByIdAsync(id, cancellationToken);
            return post is null ? null : _mapper.Map<PostDto>(post);
        }

        public async Task<PostDto> CreateAsync(CreatePostDto dto, string authorId, CancellationToken cancellationToken = default)
        {
            var post = new Post
            {
                Title = dto.Title,
                Content = dto.Content,
                AuthorId = authorId,
                CreatedAt = DateTime.UtcNow
            };

            await _postRepository.AddAsync(post, cancellationToken);
            await _postRepository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<PostDto>(post);
        }
    }
}
