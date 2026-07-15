using MiniBlog.Application.DTOs;
using MiniBlog.Application.Interfaces;
using MiniBlog.Domain.Entities;
using MiniBlog.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public async Task<(IEnumerable<PostDto> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, string? search, CancellationToken cancellationToken = default)
        {
            IQueryable<Post> query = _postRepository.GetAllQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p => p.Title.Contains(search) || p.Content.Contains(search));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var posts = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

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
