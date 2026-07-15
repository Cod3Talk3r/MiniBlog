using MiniBlog.Application.DTOs;
using MiniBlog.Application.Interfaces;
using MiniBlog.Domain.Entities;
using MiniBlog.Domain.Interfaces;

namespace MiniBlog.Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly AutoMapper.IMapper _mapper;

        public CommentService(ICommentRepository commentRepository, AutoMapper.IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CommentDto>> GetByPostIdAsync(int postId, CancellationToken cancellationToken = default)
        {
            var comments = await _commentRepository.GetByPostIdAsync(postId, cancellationToken);
            return _mapper.Map<IEnumerable<CommentDto>>(comments);
        }

        public async Task<CommentDto> AddAsync(int postId, CreateCommentDto dto, string authorId, CancellationToken cancellationToken = default)
        {
            var comment = new Comment
            {
                PostId = postId,
                Text = dto.Text,
                AuthorId = authorId,
                CreatedAt = DateTime.UtcNow
            };

            await _commentRepository.AddAsync(comment, cancellationToken);
            await _commentRepository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CommentDto>(comment);
        }
    }
}